namespace FluentEmailCore
{
    using EmailFluentCore;
    using MailKit.Net.Smtp;
    using MimeKit;
    using NETCore.MailKit;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFluentEmail : IDisposable
    {
        MimeMessage Message { get; set; }
        IFluentEmail UsingClient(SmtpClient client);
        IFluentEmail To(string emailAddress, string name);
        IFluentEmail To(string emailAddress);
        IFluentEmail To(IList<MailboxAddress> mailAddresses);
        IFluentEmail CC(string emailAddress, string name = "");
        IFluentEmail CC(IList<MailboxAddress> mailAddresses);
        IFluentEmail BCC(string emailAddress, string name = "");
        IFluentEmail BCC(IList<MailboxAddress> mailAddresses);
        IFluentEmail ReplyTo(string address);
        IFluentEmail ReplyTo(string address, string name);
        IFluentEmail Subject(string subject);
        IFluentEmail HtmlBody(string htmlBody);
        IFluentEmail TextBody(string textBody);
        IFluentEmail HighPriority();
        IFluentEmail LowPriority();
        IFluentEmail Attach(Attachment attachment);
        IFluentEmail Attach(IList<Attachment> attachments);
        IFluentEmail UsingTemplateEngine(ITemplateRenderer renderer);
        IFluentEmail UsingTemplateFromEmbedded<T>(string path, T model, Assembly assembly, bool isHtml = true);
        IFluentEmail UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true);
        IFluentEmail UsingTemplate<T>(string template, T model, bool isHtml = true);
        IFluentEmail Send();
        Task<IFluentEmail> SendAsync(CancellationToken token = default(CancellationToken));
        IFluentEmail Cancel();

    }
}
