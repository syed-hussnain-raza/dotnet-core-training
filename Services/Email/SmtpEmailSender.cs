using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MyAssignment.Options;
using Microsoft.Extensions.Logging;
using MyAssignment.Constants;

namespace MyAssignment.Services.Email
{
    /// <summary>
    /// Sends emails via SMTP (MailKit), using connection details appsettings.json; 
    /// Password comes from the Smtp__Password in .env
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IOptions<SmtpSettings> smtpOptions, ILogger<SmtpEmailSender> logger)
        {
            _smtpSettings = smtpOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using MimeMessage message = new MimeMessage 
            { 
                Subject = subject, 
                Body = new TextPart(MessagesConstants.HtmlFormat) { Text = htmlBody } 
            };
            message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));

            using SmtpClient client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, MessagesConstants.EmailSendFailedLog, toEmail);
                throw;
            }
            finally
            {
                if (client.IsConnected) await client.DisconnectAsync(true);
            }
        }
    }
}
