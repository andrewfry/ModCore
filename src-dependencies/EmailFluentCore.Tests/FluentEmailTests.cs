using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentEmailCore;
using MimeKit;
using Xunit;

namespace EmailFluentCore.Tests
{
    public class FluentEmailTests
    {
        const string toEmail = "bob@test.com";
        const string fromEmail = "johno@test.com";
        const string subject = "sup dawg";
        const string body = "what be the hipitity hap?";

        [Fact]
        public void Set_Custom_Template()
        {
            string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";

            var email = Email
                .From(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplateEngine(new TestTemplate())
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Assert.Equal("custom template", (email.Message.HtmlBody));
        }


        [Fact]
        public void Razor_Template()
        {
            string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";

            var email = Email
                .From(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .UsingTemplate(template, new { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } });

            Assert.Equal("sup LUKE here is a list 123", (email.Message.HtmlBody));
        }

        [Fact]
        public void To_Address_Is_Set()
        {
            var email = Email
                .From(fromEmail)
                .To(toEmail);

            Assert.Equal(toEmail, (email.Message.To[0] as MailboxAddress).Address);
        }

        [Fact]
        public void From_Address_Is_Set()
        {
            var email = Email.From(fromEmail);

            Assert.Equal(fromEmail, (email.Message.From.First() as MailboxAddress).Address);
        }

        [Fact]
        public void Subject_Is_Set()
        {
            var email = Email
                .From(fromEmail)
                .Subject(subject);

            Assert.Equal(subject, email.Message.Subject);
        }

        [Fact]
        public void Body_Is_Set()
        {
            var email = Email.From(fromEmail)
                .TextBody(body);

            Assert.Equal(body, (email.Message.Body as TextPart).Text);
        }

        [Fact]
        public void Can_Add_Multiple_Recipients()
        {
            string toEmail1 = "bob@test.com";
            string toEmail2 = "ratface@test.com";

            var email = Email
                .From(fromEmail)
                .To(toEmail1)
                .To(toEmail2);

            Assert.Equal(2, email.Message.To.Count);
        }

        [Fact]
        public void Can_Add_Multiple_Recipients_From_List()
        {
            var emails = new List<MailboxAddress>();
            emails.Add(new MailboxAddress("email1@email.com"));
            emails.Add(new MailboxAddress("email2@email.com"));

            var email = Email
                .From(fromEmail)
                .To(emails);

            Assert.Equal(2, email.Message.To.Count);
        }

        [Fact]
        public void Can_Add_Multiple_CCRecipients_From_List()
        {
            var emails = new List<MailboxAddress>();
            emails.Add(new MailboxAddress("email1@email.com"));
            emails.Add(new MailboxAddress("email2@email.com"));

            var email = Email
                .From(fromEmail)
                .CC(emails);

            Assert.Equal(2, email.Message.Cc.Count);
        }

        [Fact]
        public void Can_Add_Multiple_BCCRecipients_From_List()
        {
            var emails = new List<MailboxAddress>();
            emails.Add(new MailboxAddress("email1@email.com"));
            emails.Add(new MailboxAddress("email2@email.com"));

            var email = Email
                .From(fromEmail)
                .BCC(emails);

            Assert.Equal(2, email.Message.Bcc.Count);
        }

        [Fact]
        public void Is_Valid_With_Properties_Set()
        {
            var email = Email
                .From(fromEmail)
                .To(toEmail)
                .Subject(subject)
                .TextBody(body);

            Assert.Equal(body, (email.Message.Body as TextPart).Text);
            Assert.Equal(subject, email.Message.Subject);
            Assert.Equal(fromEmail, (email.Message.From.First() as MailboxAddress).Address);
            Assert.Equal(toEmail, (email.Message.To[0] as MailboxAddress).Address);
        }

        [Fact]
        public void ReplyTo_Address_Is_Set()
        {
            var replyEmail = "reply@email.com";

            var email = Email.From(fromEmail)
                .ReplyTo(replyEmail);

            Assert.Equal(replyEmail, (email.Message.ReplyTo.First() as MailboxAddress).Address);
        }
    }


    public class TestTemplate : ITemplateRenderer
    {
        public string Parse<T>(string template, T model, bool isHtml = true)
        {
            return "custom template";
        }
    }

    public class TestClass
        {
        public string Name { get; set; }
        public string[] Numbers { get; set; }
    }

}
