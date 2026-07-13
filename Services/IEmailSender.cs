namespace MyAssignment.Services
{
    /// <summary>
    /// Sends transactional emails (e.g. account confirmation links).
    /// </summary>
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);
    }
}