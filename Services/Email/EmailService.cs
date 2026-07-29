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
        private readonly IOptions<UrlSettings> _urlSettings;

        public EmailService(IEmailSender emailSender, IOptions<UrlSettings> urlSettings)
        {
            _emailSender = emailSender;
            _urlSettings = urlSettings;
        }

        public async Task SendConfirmationEmailAsync(User user, string emailConfirmationToken)
        {
            if (user.Email == null) return;

            string confirmationLink = $"{_urlSettings.Value.ConfirmEmailUrl}?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(emailConfirmationToken)}";
            string body = EmailTemplates.GetConfirmationEmailTemplate(confirmationLink);

            await _emailSender.SendEmailAsync(user.Email, MessagesConstants.EmailSubjectConfirmAccount, body);
        }
    }
}
