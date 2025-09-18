using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SocialMedia.BLL.ModelVM.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.Service.Implementation
{
    public class EmailSerives : IEmailService
    {
        //private readonly EmailSettings _settings;

        //public EmailSerives(IOptions<EmailSettings> settings)
        //{
        //    _settings = settings.Value;
        //}

        //public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        //    message.To.Add(MailboxAddress.Parse(toEmail));
        //    message.Subject = subject;

        //    var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        //    message.Body = bodyBuilder.ToMessageBody();

        //    using var client = new SmtpClient();
        //    // Connect - use StartTls for port 587
        //    await client.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        //    await client.AuthenticateAsync(_settings.Username, _settings.Password);
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}

        private readonly EmailSettings _settings;

        public EmailSerives(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
