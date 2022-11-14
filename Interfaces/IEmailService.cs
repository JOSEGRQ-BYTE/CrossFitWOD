using System;
using CrossFitWOD.Models;

namespace CrossFitWOD.Interfaces
{
    public interface IEmailService
    {
        Task SendPlainTextMessageAsync(EmailRequest mailRequest);
        Task SendVerificationEmailAsync(string link, string receiver);
        Task SendResetPasswordLinkAsync(string link, User receiver);
    }
}

