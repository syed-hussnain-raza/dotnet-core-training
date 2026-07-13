using MyAssignment.Services;

namespace MyAssignment.Extensions
{
    /// <summary>
    /// Registers the SMTP email sender used for account confirmation emails.
    /// </summary>
    public static class EmailServiceExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            return services;
        }
    }
}