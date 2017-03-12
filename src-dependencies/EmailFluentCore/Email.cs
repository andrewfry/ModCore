using EmailFluentCore;
using MailKit.Net.Smtp;
using MimeKit;
using NETCore.MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Runtime.Loader;

namespace FluentEmailCore
{


    public class Email : IFluentEmail
    {
        private SmtpClient _client;
        private SslProtocols? _useSsl;
        private bool _bodyIsHtml = true;
        private ITemplateRenderer _renderer;
        private BodyBuilder _bodyBuilder;

        public MimeMessage Message { get; set; }

        public static ITemplateRenderer DefaultRenderer { get; set; }

        /// <summary>
        /// Creates a new email instance using the default from
        /// address from smtp config settings
        /// </summary>
        public Email() : this(new SmtpClient()) { }

        /// <summary>
        /// Creates a new email instance with overrides the default client from .config file.
        /// </summary>
        /// <param name="client">Smtp client to send from</param>
        public Email(SmtpClient client) : this(client, new RazorRenderer()) { }

        /// <summary>
        /// Creates a new email instance with overrides the default client from .config file
        /// and the template rendering, defaults to RazorEngine.
        /// </summary>
        /// <param name="client">Smtp client to send from</param>
        /// <param name="defaultRenderer">The template rendering engine</param>
        public Email(SmtpClient client, ITemplateRenderer defaultRenderer)
        {
            _client = client;
            _renderer = defaultRenderer;
            Message = new MimeMessage();
            _bodyBuilder = new BodyBuilder();

        }

        /// <summary>
        /// Creates a new Email instance and sets the from property using default Smtp client from .config file
        /// and defaults template rendering: RazorEngine.
        /// </summary>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(string emailAddress, string name = "") : this(new SmtpClient(), new RazorRenderer(), emailAddress, name) { }

        /// <summary>
        ///  Creates a new Email instance and sets the from property with overrides the default client from .config
        /// </summary>
        /// <param name="client">Smtp client to send from</param>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(SmtpClient client, string emailAddress, string name = "") : this(client, new RazorRenderer(), emailAddress, name) { }

        /// <summary>
        ///  Creates a new Email instance and sets the from property with overrides the default  filetemplate rendering: RazorEngine.
        /// </summary>
        /// <param name="defaultRenderer">The template rendering engine</param>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(ITemplateRenderer defaultRenderer, string emailAddress, string name = "") : this(new SmtpClient(), defaultRenderer, emailAddress, name) { }

        /// <summary>
        /// Creates a new Email instance and sets the from property with overrides the default client from .config and filetemplate rendering: RazorEngine.
        /// </summary>
        /// <param name="client">Smtp client to send from</param>
        /// <param name="defaultRenderer">The template rendering engine</param>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(SmtpClient client, ITemplateRenderer defaultRenderer, string emailAddress, string name = "")
            : this(client, defaultRenderer)
        {
            Message.From.Add(new MailboxAddress(name, emailAddress));
        }


        /// <summary>
        /// Creates a new Email instance and sets the from
        /// property
        /// </summary>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        /// <returns>Instance of the Email class</returns>
        public static IFluentEmail From(string emailAddress, string name = "")
        {
            return new Email(emailAddress, name);
        }

        /// <summary>
        /// Adds a reciepient to the email, Splits name and address on ';'
        /// </summary>
        /// <param name="emailAddress">Email address of recipeient</param>
        /// <param name="name">Name of recipient</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail To(string emailAddress, string name)
        {
            if (emailAddress.Contains(";"))
            {
                //email address has semi-colon, try split
                var nameSplit = name.Split(';');
                var addressSplit = emailAddress.Split(';');
                for (int i = 0; i < addressSplit.Length; i++)
                {
                    var currentName = string.Empty;
                    if ((nameSplit.Length - 1) >= i)
                    {
                        currentName = nameSplit[i];
                    }
                    Message.To.Add(new MailboxAddress(currentName, addressSplit[i]));
                }
            }
            else
            {
                Message.To.Add(new MailboxAddress(name, emailAddress));
            }
            return this;
        }

        /// <summary>
        /// Adds a reciepient to the email
        /// </summary>
        /// <param name="emailAddress">Email address of recipeient (allows multiple splitting on ';')</param>
        /// <returns></returns>
        public IFluentEmail To(string emailAddress)
        {
            if (emailAddress.Contains(";"))
            {
                foreach (string address in emailAddress.Split(';'))
                {
                    Message.To.Add(new MailboxAddress(address));
                }
            }
            else
            {
                Message.To.Add(new MailboxAddress(emailAddress));
            }

            return this;
        }

        /// <summary>
        /// Adds all reciepients in list to email
        /// </summary>
        /// <param name="mailAddresses">List of recipients</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail To(IList<MailboxAddress> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Message.To.Add(address);
            }
            return this;
        }

        /// <summary>
        /// Adds a Carbon Copy to the email
        /// </summary>
        /// <param name="emailAddress">Email address to cc</param>
        /// <param name="name">Name to cc</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail CC(string emailAddress, string name = "")
        {
            Message.Cc.Add(new MailboxAddress(emailAddress, name));
            return this;
        }

        /// <summary>
        /// Adds all Carbon Copy in list to an email
        /// </summary>
        /// <param name="mailAddresses">List of recipients to CC</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail CC(IList<MailboxAddress> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Message.Cc.Add(address);
            }
            return this;
        }

        /// <summary>
        /// Adds a blind carbon copy to the email
        /// </summary>
        /// <param name="emailAddress">Email address of bcc</param>
        /// <param name="name">Name of bcc</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail BCC(string emailAddress, string name = "")
        {
            Message.Bcc.Add(new MailboxAddress(name, emailAddress));
            return this;
        }

        /// <summary>
        /// Adds all blind carbon copy in list to an email
        /// </summary>
        /// <param name="mailAddresses">List of recipients to BCC</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail BCC(IList<MailboxAddress> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Message.Bcc.Add(address);
            }
            return this;
        }

        /// <summary>
        /// Sets the ReplyTo address on the email
        /// </summary>
        /// <param name="address">The ReplyTo Address</param>
        /// <returns></returns>
        public IFluentEmail ReplyTo(string address)
        {
            Message.ReplyTo.Add(new MailboxAddress(address));

            return this;
        }

        /// <summary>
        /// Sets the ReplyTo address on the email
        /// </summary>
        /// <param name="address">The ReplyTo Address</param>
        /// <param name="name">The Display Name of the ReplyTo</param>
        /// <returns></returns>
        public IFluentEmail ReplyTo(string address, string name)
        {
            Message.ReplyTo.Add(new MailboxAddress(name, address));

            return this;
        }

        /// <summary>
        /// Sets the subject of the email
        /// </summary>
        /// <param name="subject">email subject</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail Subject(string subject)
        {
            Message.Subject = subject;
            return this;
        }

        /// <summary>
        /// Adds a Body to the Email
        /// </summary>
        /// <param name="body">The content of the body</param>
        /// <param name="isHtml">True if Body is HTML, false for plain text (Optional)</param>
        public IFluentEmail HtmlBody(string htmlBody)
        {
            _bodyBuilder.HtmlBody = htmlBody;
            Message.Body = _bodyBuilder.ToMessageBody();
            return this;
        }

        public IFluentEmail TextBody(string textBody)
        {
            _bodyBuilder.TextBody = textBody;
            Message.Body = _bodyBuilder.ToMessageBody();
            return this;
        }

        /// <summary>
        /// Marks the email as High Priority
        /// </summary>
        public IFluentEmail HighPriority()
        {
            Message.Priority = MessagePriority.Urgent;
            return this;
        }

        /// <summary>
        /// Marks the email as Low Priority
        /// </summary>
        public IFluentEmail LowPriority()
        {
            Message.Priority = MessagePriority.NonUrgent;
            return this;
        }

        /// <summary>
        /// Set the template rendering engine to use, defaults to RazorEngine
        /// </summary>
        public IFluentEmail UsingTemplateEngine(ITemplateRenderer renderer)
        {
            _renderer = renderer;
            return this;
        }

        /// <summary>
        /// Adds template to email from embedded resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path the the embedded resource eg [YourAssembly].[YourResourceFolder].[YourFilename.txt]</param>
        /// <param name="model">Model for the template</param>
        /// <param name="assembly">The assembly your resource is in. Defaults to calling assembly.</param>
        /// <returns></returns>
        public IFluentEmail UsingTemplateFromEmbedded<T>(string path, T model, Assembly assembly, bool isHtml = true)
        {
            CheckRenderer();


            var template = EmbeddedResourceHelper.GetResourceAsString(assembly, path);
            var result = _renderer.Parse(template, model, isHtml);

            if (isHtml)
                _bodyBuilder.HtmlBody = result;
            else
                _bodyBuilder.TextBody = result;


            return this;
        }

        /// <summary>
        /// Adds the template file to the email
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        /// <param name="isHtml">True if Body is HTML, false for plain text (Optional)</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true)
        {
            var path = GetFullFilePath(filename);
            var template = "";

            using (StreamReader reader = File.OpenText(path))
            {
                template = reader.ReadToEnd();
            }


            CheckRenderer();

            var result = _renderer.Parse(template, model, _bodyIsHtml);

            if (isHtml)
                _bodyBuilder.HtmlBody = result;
            else
                _bodyBuilder.TextBody = result;

            Message.Body = _bodyBuilder.ToMessageBody();

            return this;
        }



        /// <summary>
        /// Adds razor template to the email
        /// </summary>
        /// <param name="template">The razor template</param>
        /// <param name="isHtml">True if Body is HTML, false for plain text (Optional)</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail UsingTemplate<T>(string template, T model, bool isHtml = true)
        {
            CheckRenderer();

            var result = _renderer.Parse(template, model, isHtml);

            if (isHtml)
                _bodyBuilder.HtmlBody = result;
            else
                _bodyBuilder.TextBody = result;

            Message.Body = _bodyBuilder.ToMessageBody();

            return this;
        }

        /// <summary>
        /// Adds an Attachment to the Email
        /// </summary>
        /// <param name="attachment">The Attachment to add</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail Attach(Attachment attachment)
        {
            _bodyBuilder.Attachments.Add(attachment.FileName);
            Message.Body = _bodyBuilder.ToMessageBody();
            return this;
        }

        /// <summary>
        /// Adds Multiple Attachments to the Email
        /// </summary>
        /// <param name="attachments">The List of Attachments to add</param>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail Attach(IList<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                Attach(attachment);
            }

            Message.Body = _bodyBuilder.ToMessageBody();
            return this;
        }

        /// <summary>
        /// Over rides the default client from .config file
        /// </summary>
        /// <param name="client">Smtp client to send from</param>
        /// <returns>Instance of the Email class</returns>
        /// [Obsolete("FluentEmail.Email.From.UsingClient(SmtpClient client) is obsolete: 'Please user the constructor'")]
        public IFluentEmail UsingClient(SmtpClient client)
        {
            _client = client;
            return this;
        }

        public IFluentEmail UseSslProtocol(SslProtocols sslProtocol)
        {
            _useSsl = sslProtocol;
            return this;
        }

        /// <summary>
        /// Sends email synchronously
        /// </summary>
        /// <returns>Instance of the Email class</returns>
        public virtual IFluentEmail Send()
        {
            if (_useSsl.HasValue)
                _client.SslProtocols = _useSsl.Value;

            // Message.IsBodyHtml = _bodyIsHtml;

            _client.Send(Message);
            return this;
        }

        /// <summary>
        /// Sends message asynchronously with a callback
        /// handler
        /// </summary>
        /// <param name="callback">Method to call on complete</param>
        /// <param name="token">User token to pass to callback</param>
        /// <returns>Instance of the Email class</returns>
        public virtual async Task<IFluentEmail> SendAsync(CancellationToken token = default(CancellationToken))
        {
            if (_useSsl.HasValue)
                _client.SslProtocols = _useSsl.Value;

            await _client.SendAsync(Message, token);

            return this;
        }

        /// <summary>
        /// Cancels async message sending
        /// </summary>
        /// <returns>Instance of the Email class</returns>
        public IFluentEmail Cancel()
        {
            _client.DisconnectAsync(true);
            return this;
        }

        /// <summary>
        /// Releases all resources
        /// </summary>
        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();

        }

        private void CheckRenderer()
        {
            if (_renderer != null) return;

            if (DefaultRenderer != null)
            {
                _renderer = DefaultRenderer;
            }
            else
            {
                _renderer = new RazorRenderer();
            }
        }

        private static string GetFullFilePath(string filename)
        {
            if (filename.StartsWith("~"))
            {

                var baseDir = System.AppContext.BaseDirectory;
                return Path.GetFullPath(baseDir + filename.Replace("~", ""));
            }

            return Path.GetFullPath(filename);
        }

        private static string GetCultureFileName(string fileName, CultureInfo culture)
        {
            var fullFilePath = GetFullFilePath(fileName);
            var extension = Path.GetExtension(fullFilePath);
            var cultureExtension = string.Format("{0}{1}", culture.Name, extension);

            var cultureFile = Path.ChangeExtension(fullFilePath, cultureExtension);
            if (File.Exists(cultureFile))
                return cultureFile;
            else
                return fullFilePath;
        }
    }
}
