using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MyAssignment.Services
{
    /// <summary>
    /// Sends real emails via SMTP (MailKit), using connection details from
    /// the "Smtp" configuration section. Host/Port/Username/FromEmail come
    /// from appsettings.json; Password comes from the Smtp__Password
    /// environment variable (.env), never committed to source control.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            string host = _configuration["Smtp:Host"] ?? string.Empty;
            int port = int.Parse(_configuration["Smtp:Port"] ?? "587");
            string username = _configuration["Smtp:Username"] ?? string.Empty;
            string password = _configuration["Smtp:Password"] ?? string.Empty;
            string fromEmail = _configuration["Smtp:FromEmail"] ?? string.Empty;
            string fromName = _configuration["Smtp:FromName"] ?? string.Empty;

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using SmtpClient client = new SmtpClient();

            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}