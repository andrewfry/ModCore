using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.Communication;
using ModCore.Models.Enum;
using ModCore.Services.MongoDb.Base;
using ModCore.Specifications.Base;
using ModCore.Specifications.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Communication
{
    public class EmailService : BaseServiceAsync<EmailTemplate>
    {
        private IDataRepositoryAsync<EmailLayout> _emailLayoutRepository;
        private IDataRepositoryAsync<SentEmail> _sentEmailRepository;

        private IOptions<EmailSettings> _settings;



        public EmailService(IDataRepositoryAsync<EmailLayout> emailLayoutRepository, IOptions<EmailSettings> settings,
            IDataRepositoryAsync<EmailTemplate> repository, IMapper mapper, ILog logger)
            : base(repository, mapper, logger)
        {
            _settings = settings;
            _emailLayoutRepository = emailLayoutRepository;
        }


        public async Task<EmailTemplate> FindByName(string name)
        {
            var email = await _repository.FindAsync(new EmailByName(name));

            if (email == null)
                throw new Exception($"Can not find the email with the following name: {name}");

            return email;
        }

        public async Task<bool> SendEmail(string emailName)
        {
            var emailTemplate = await FindByName(emailName);
            var emailMessage = _mapper.Map<EmailMessage>(emailTemplate);

            if (emailTemplate.HasLayout())
            {
                var layout = await _emailLayoutRepository.FindByIdAsync(emailTemplate.EmailLayoutId);
                emailMessage.Body = ReplaceBody(emailMessage.Body, layout.Html);
            }

            return false;
        }

        private async Task<bool> PrepareAndSend(EmailMessage email, object objectToBind)
        {
            var propValueDict = new Dictionary<string, string>();
            var typeInfo = objectToBind.GetType().GetTypeInfo();
            var propList = typeInfo.GetProperties().ToList();

            foreach (var prop in propList)
            {
                var value = prop.GetValue(objectToBind);

                propValueDict.Add(prop.Name, value.ToString());
            }

            return await PrepareAndSend(email, propValueDict);
        }

        private async Task<bool> PrepareAndSend(EmailMessage email, Dictionary<string, string> replacements)
        {

            var emailServer = _settings.Value.EmailServer;
            var emailServerPort = _settings.Value.EmailServerPort;
            var emailServerUserName = _settings.Value.EmailServerPort;
            var emailServerUserPassword = _settings.Value.EmailServerUserPassword;
            var emailSecure = _settings.Value.EmailServerSecure;
            var emailTestMode = _settings.Value.TestMode;
            var emailTestEmailAddress = _settings.Value.TestEmailAddress;
            var emailLocalDomain = _settings.Value.LocalDomain;

            var mailMessage = new MimeMessage(new MailboxAddress(email.From.DisplayName, email.From.EmailAddress), new List<MailboxAddress>(), "", null);

            if (emailTestMode)
            {

                StringBuilder to = new StringBuilder();
                StringBuilder cc = new StringBuilder();
                StringBuilder bcc = new StringBuilder();
                StringBuilder message = new StringBuilder();

                foreach (var ad in email.To)
                {
                    to.Append(ad.EmailAddress + ";");
                }

                foreach (var ad in email.Cc)
                {
                    cc.Append(ad.EmailAddress + ";");
                }

                foreach (var ad in email.Bcc)
                {
                    bcc.Append(ad.EmailAddress + ";");
                }

                message.Append("<table><tr><td colspan=\"2\">This message was not sent in a production environment. This message would normally be sent to the following addresses:</td></tr><tr><td>To:</td><td>");
                message.Append(to.ToString());
                message.Append("</td></tr><tr><td>CC:</td><td>");
                message.Append(cc.ToString());
                message.Append("</td></tr><tr><td>BCC:</td><td>");
                message.Append(bcc.ToString());
                message.Append("</td></tr></table>");

                email.To.Clear();
                email.Cc.Clear();
                email.Bcc.Clear();

                email.To.Add(new EmailContact(emailTestEmailAddress));

                email.Body = email.Body + "<br /><br />" + message.ToString();

            }
            else
            {
                foreach (var ad in email.To)
                {
                    mailMessage.To.Add(new MailboxAddress(ad.DisplayName, ad.EmailAddress));
                }

                foreach (var ad in email.Cc)
                {
                    mailMessage.To.Add(new MailboxAddress(ad.DisplayName, ad.EmailAddress));
                }

                foreach (var ad in email.Bcc)
                {
                    mailMessage.To.Add(new MailboxAddress(ad.DisplayName, ad.EmailAddress));
                }
            }

            var htmlBody = new TextPart(TextFormat.Html);
            htmlBody.Text = ReplaceVariables(email.Body, replacements);

            mailMessage.Body = htmlBody;
            mailMessage.Subject = ReplaceVariables(email.Subject, replacements);




            using (var client = new SmtpClient())
            {
                client.LocalDomain = emailLocalDomain;

                var secure = SecureSocketOptions.None;

                if (emailSecure)
                    secure = SecureSocketOptions.Auto;

                //TODO smtp clients with authentication

                await client.ConnectAsync(emailServer, emailServerPort, secure).ConfigureAwait(false);
                await client.SendAsync(mailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }


            return true;

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
            //TODO add the ability to add "system variables" like:
            //1. SiteName
            //2. SiteURl

            return replacements;
        }

        protected string ReplaceBody(string emailTemplateBody, string layoutHtml)
        {
            MatchCollection matches;
            matches = Regex.Matches(layoutHtml, "\\@\\@.*?\\@\\@");
            if (matches.Count > 0)
            {

                emailTemplateBody = layoutHtml.Replace("@@RenderBody@@", emailTemplateBody);

            }
            return emailTemplateBody;
        }

        private async Task CreateSentEmail(EmailMessage email, EmailSentStatus status)
        {
            var sentEmail = _mapper.Map<SentEmail>(email);

            sentEmail.DateSent = DateTime.UtcNow;
            sentEmail.Status = status;

            await _sentEmailRepository.InsertAsync(sentEmail);

        }

    }


    public static class BuiltInEmails
    {
        public static string EmailVerification => "Builtin-EmailVerification";

        public static string ResetPassworVerification => "Builtin-ResetPassworVerification";

        public static string ForgotPassword => "Builtin-ForgotPassword";
    }


}
