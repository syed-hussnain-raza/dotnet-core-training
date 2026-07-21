using Microsoft.Extensions.Options;
using MyAssignment.Constants;
using MyAssignment.Helper;
using MyAssignment.Models;
using MyAssignment.Options;

namespace MyAssignment.Services.Email
{
    /// <summary>
    /// Implementation of IEmailService that constructs URLs, formats HTML templates, and dispatches via IEmailSender.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IOptions<FrontendSettings> _frontendSettings;

        public EmailService(IEmailSender emailSender, IOptions<FrontendSettings> frontendSettings)
        {
            _emailSender = emailSender;
            _frontendSettings = frontendSettings;
        }

        public async Task SendConfirmationEmailAsync(User user, string emailConfirmationToken)
        {
            if (user.Email == null) return;

            string confirmationLink = $"{_frontendSettings.Value.ConfirmEmailUrl}?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(emailConfirmationToken)}";
            string body = EmailTemplates.GetConfirmationEmailTemplate(confirmationLink);

            await _emailSender.SendEmailAsync(user.Email, MessagesConstants.EmailSubjectConfirmAccount, body);
        }

        public async Task SendPasswordSetEmailAsync(User user, string passwordResetToken)
        {
            if (user.Email == null) return;

            string setPasswordLink = $"{_frontendSettings.Value.SetPasswordUrl}?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(passwordResetToken)}";
            string body = EmailTemplates.GetSetPasswordEmailTemplate(setPasswordLink);

            await _emailSender.SendEmailAsync(user.Email, MessagesConstants.EmailSubjectSetPassword, body);
        }
    }
}
