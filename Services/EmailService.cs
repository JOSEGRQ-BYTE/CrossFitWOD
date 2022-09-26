using System;
using System.Net;
using MailKit.Net.Smtp;
using AutoMapper;
using CrossFitWOD.Models;
using MailKit;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using CrossFitWOD.Interfaces;

namespace CrossFitWOD.Controllers
{
    public class EmailService: IEmailService
    {
        private readonly EmailSettings _EmailSettings;

        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _EmailSettings = mailSettings.Value;
        }

        public async Task SendPlainTextMessageAsync(EmailRequest emailRequest)
        {

            var myEmail = _EmailSettings;
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_EmailSettings.Email),
                Subject = "Message Request!",
            };
            email.To.Add(MailboxAddress.Parse(_EmailSettings.ToEmail));

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<strong>This is: </strong>{emailRequest.FirstName} {emailRequest.LastName}" +
                $"<br/>" +
                $"<strong>Email: </strong>{emailRequest.Email}" +
                $"<br/>" +
                $"<strong>Message: </strong>" +
                $"<p>{emailRequest.Message}</p>";
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_EmailSettings.Host, _EmailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_EmailSettings.Email, _EmailSettings.Password);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}

