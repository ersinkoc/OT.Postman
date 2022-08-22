using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace OT.Postman
{
    public class EmailService : IEmailSender
    {
        public Task SendEmailAsync(Constants emailProvider, string? recipientName, string recipientEmail, string subject, string message)
        {
            string _recipientName = recipientEmail;
            if (!string.IsNullOrEmpty(recipientName))
            {
                _recipientName = recipientEmail;
            }
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(emailProvider.FromName, emailProvider.FromMail));
            mimeMessage.To.Add(new MailboxAddress(_recipientName, recipientEmail));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message,
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(emailProvider.SmtpHost, emailProvider.SmtpPort, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailProvider.FromMail, emailProvider.EmailPassword);
                client.Send(mimeMessage);
                client.Disconnect(true);
                return Task.FromResult(0);
            }
        }
    }
}
