using MyAssignment.Models;

namespace MyAssignment.Services.Email
{
    /// <summary>
    /// Handles the business logic for constructing and dispatching application emails.
    /// </summary>
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(User user, string emailConfirmationToken);
    }
}
