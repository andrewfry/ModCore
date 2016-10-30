using Microsoft.Extensions.Options;
using ModCore.Abstraction.DataAccess;
using ModCore.Models.Communication;
using ModCore.Specifications.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Communication
{
    public class EmailService
    {
        private IDataRepository<Email> _repository;
        private IOptions<EmailSettings> _settings;



        public EmailService(IDataRepository<Email> repository, IOptions<EmailSettings> settings)
        {
            _repository = repository;
            _settings = settings;
        }


        public Email FindByName(string name)
        {
            var email = _repository.Find(new EmailByName(name));

            if (email == null)
                throw new Exception($"Can not find the email with the following name: {name}");

            return email;
        }

        public bool PrepareAndSend(Email email, string currentUserId)
        {
            return PrepareAndSend(email, currentUserId, null);
        }

        public bool PrepareAndSend(string emailName, string to, string currentUserId, Dictionary<string, string> replacements)
        {
            var email = FindByName(emailName);
            email.To.Clear();
            email.Cc.Clear();
            email.Bcc.Clear();

            email.To.Add(to);

            return PrepareAndSend(email, currentUserId, replacements);
        }

        public bool PrepareAndSend(string emailName, string currentUserId, Dictionary<string, string> replacements)
        {
            var email = FindByName(emailName);
            return PrepareAndSend(email, currentUserId, replacements);
        }

        public bool PrepareAndSend(Email email, string currentUserId, Dictionary<string, string> replacements)
        {

            var emailServer = _settings.Value.EmailServer;
            var emailServerPort = _settings.Value.EmailServerPort;
            var emailServerUserName = _settings.Value.EmailServerPort;
            var emailServerUserPassword = ConfigurationManager.AppSettings["emailServerUserPassword"].ToString();
            var emailSecure = Convert.ToBoolean(ConfigurationManager.AppSettings["emailServerSecure"]);

            email.SendEmail = ConfigurationManager.AppSettings["SendEmail"].ToString();

            MailMessage mailMessage = new MailMessage();

            if (email.SendEmail == "Local" && !string.IsNullOrEmpty(currentUserId))
            {

                var testingEmailAddress = ConfigurationManager.AppSettings["emailTestEmail"].ToString();
                StringBuilder to = new StringBuilder();
                StringBuilder cc = new StringBuilder();
                StringBuilder bcc = new StringBuilder();
                StringBuilder message = new StringBuilder();

                foreach (var ad in email.To)
                {
                    to.Append(ad + ";");
                }

                foreach (var ad in email.Cc)
                {
                    cc.Append(ad + ";");
                }

                foreach (var ad in email.Bcc)
                {
                    bcc.Append(ad + ";");
                }

                message.Append("<table><tr><td colspan=\"2\">This message was not sent in a production environment. This message would normally be sent to the following addresses:</td></tr><tr><td>To:</td><td>");
                message.Append(to.ToString());
                message.Append("</td></tr><tr><td>CC:</td><td>");
                message.Append(cc.ToString());
                message.Append("</td></tr><tr><td>BCC:</td><td>");
                message.Append(bcc.ToString());
                message.Append("</td></tr></table>");

                email.To.Clear();
                email.To.Add(testingEmailAddress);
                email.Cc.Clear();
                email.Cc.Clear();

                email.Body = email.Body + "<br /><br />" + message.ToString();


                var user = _userService.Find(currentUserId);

                email.To.Clear();
                email.To.Add(user.EmailAddress);
                email.Cc.Clear();
            }
            else
            {
                foreach (var ad in email.To)
                {
                    mailMessage.To.Add(new MailAddress(ad));
                }

                foreach (var ad in email.Cc)
                {
                    mailMessage.CC.Add(new MailAddress(ad));
                }

                foreach (var ad in email.Bcc)
                {
                    mailMessage.Bcc.Add(new MailAddress(ad));
                }
            }


            mailMessage.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(email.EmailLayoutId))
            {
                email.Body = ReplaceBody(email.Body, email.EmailLayoutId);
            }

            email.Body = ReplaceVariables(email.Body, replacements);
            email.Subject = ReplaceVariables(email.Subject, replacements);

            mailMessage.Body = email.Body;
            mailMessage.Subject = email.Subject;

            mailMessage.From = new MailAddress(email.From);
            email.MailMessage = mailMessage;

            using (SmtpClient smtpClient = new SmtpClient(emailServer, emailServerPort))
            {
                smtpClient.Credentials = new NetworkCredential(emailServerUserName, emailServerUserPassword);

                if (emailSecure)
                    smtpClient.EnableSsl = true;

                email.SMTPMailServer = smtpClient;
                SendThroughSmtp(email);
            }

            return true;

        }

        public bool PrepareAndSend(Email email, Dictionary<string, string> replacements)
        {
            return PrepareAndSend(email, "", replacements);
        }

        public bool PrepareAndSend(string emailName, Dictionary<string, string> replacements)
        {
            var email = this.FindByName(emailName);
            return PrepareAndSend(email, replacements);
        }

        protected string ReplaceVariables(string input, Dictionary<string, string> replacements)
        {

            replacements = AddSystemVariables(replacements);

            MatchCollection matches;
            matches = Regex.Matches(input, "\\#\\#.*?\\#\\#");
            if (matches.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in replacements)
                {
                    input = input.Replace("##" + entry.Key + "##", entry.Value);
                }
            }
            return input;
        }

        protected Dictionary<string, string> AddSystemVariables(Dictionary<string, string> replacements)
        {
            if (replacements.All(a => a.Key != "CURRENT_URL"))
            {
                if (ConfigurationManager.AppSettings["CURRENT_URL"] != null)
                {
                    replacements.Add("CURRENT_URL", ConfigurationManager.AppSettings["CURRENT_URL"].ToString());
                }
            }

            return replacements;
        }

        protected string ReplaceBody(string body, string emailLayoutId)
        {
            var layout = _emailLayoutRepository.Find(emailLayoutId);

            MatchCollection matches;
            matches = Regex.Matches(layout.Body, "\\@\\@.*?\\@\\@");
            if (matches.Count > 0)
            {

                body = layout.Body.Replace("@@RenderBody@@", body);

            }
            return body;
        }

        protected bool SendThroughSmtp(Email email)
        {
            if (SetCustomHeaders(email))
            {
                //Send the email
                if (email.SendEmail != "No")
                {
                    //try
                    //{
                    //    //TODO Create a historical record of all emails sent - this would be a failure
                    //    email.SMTPMailServer.Send(email.MailMessage);
                    //    CreateSentEmail(email, EmailSentStatus.Success);
                    //}
                    //catch (Exception ex)
                    //{
                    //    CreateSentEmail(email, EmailSentStatus.Failed);
                    //    throw new Exception("There was a failure sending the email.", ex);
                    //}


                    try
                    {
                        //FUCKING PAYPAL AND PCI COMPLIANCE
                        // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        email.SMTPMailServer.Send(email.MailMessage);
                        CreateSentEmail(email, EmailSentStatus.Success);
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int i = 0; i < ex.InnerExceptions.Length; i++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy ||
                                status == SmtpStatusCode.MailboxUnavailable)
                            {
                                System.Threading.Thread.Sleep(5000);
                                email.SMTPMailServer.Send(email.MailMessage);

                                CreateSentEmail(email, EmailSentStatus.Success);
                            }
                            else
                            {
                                CreateSentEmail(email, EmailSentStatus.Failed);
                                throw new Exception("There was a failure sending the email.", ex);
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SetCustomHeaders(Email email)
        {
            email.MailMessage.Headers.Remove("Disposition-Notification-To");
            if (email.ReadReceipt == true)
            {
                email.MailMessage.Headers["Disposition-Notification-To"] = email.MailMessage.From.Address;
            }
            return true;
        }

        private void CreateSentEmail(Email email, EmailSentStatus status)
        {
            var sentEmail = new SentEmail
            {
                Bcc = email.Bcc,
                Body = email.Body,
                Cc = email.Cc,
                Description = email.Description,
                From = email.From,
                Name = email.From,
                ReadReceipt = email.ReadReceipt,
                SendEmail = email.SendEmail,
                Subject = email.Subject,
                To = email.To,
                DateSent = DateTime.UtcNow,
                Status = status,
            };

            _sentEmailRepository.Insert(sentEmail);

        }


     
      
    

    }


    public static class BuiltInEmails
    {
        public static string EmailVerification => "Builtin-EmailVerification";

        public static string ResetPassworVerification => "Builtin-ResetPassworVerification";

        public static string ForgotPassword => "Builtin-ForgotPassword";
    }
}
