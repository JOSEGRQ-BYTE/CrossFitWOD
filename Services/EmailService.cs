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

        public async Task SendVerificationEmailAsync(string link, string receiver)
        {
            var draft = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_EmailSettings.Email),
                Subject = "Email Verification Required",
            };
            draft.To.Add(MailboxAddress.Parse(receiver));

            var builder = new BodyBuilder();
            builder.HtmlBody =
                $"<strong>Please click on the link below to verify email prior to login.</strong>" +
                $"<br/>" +
                $"{link}";
            draft.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_EmailSettings.Host, _EmailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_EmailSettings.Email, _EmailSettings.Password);

            await smtp.SendAsync(draft);

            smtp.Disconnect(true);
        }

        public async Task SendResetPasswordLinkAsync(string link, User receiver)
        {
            var draft = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_EmailSettings.Email),
                Subject = "Reset Password Request",
            };
            draft.To.Add(MailboxAddress.Parse(receiver.Email));

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<p>{receiver.FirstName} {receiver.LastName}," +
                $"<br/>" +
                $"We received a request to reset the password to your account linked to this email." +
                $"<br/>" +
                $"If you requested this change, please continue to this link:" +
                $"<br/>" +
                $"<br/>" +
                $"<a style=\"color: #fff;background-color: #6c757d;border-color: #6c757d;font-weight: 400;text-align: center;vertical-align: middle;cursor: pointer;border: 1px solid transparent;padding: 0.375rem 0.75rem;line-height: 0.5em;border-radius: 0.25rem;text-decoration: none;\" href=\"{link}\">Set New Password</a>" +
                $"<br/>" +
                $"<br/>" +
                $"otherwise, you may ignore this request and your account will remain intact.</p>.";
                //$"{link}";
            draft.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_EmailSettings.Host, _EmailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_EmailSettings.Email, _EmailSettings.Password);

            await smtp.SendAsync(draft);

            smtp.Disconnect(true);
        }
    }
}

