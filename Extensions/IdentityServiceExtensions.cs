using Microsoft.AspNetCore.Identity;
using MyAssignment.Data;

namespace MyAssignment.Extensions
{
    /// <summary>
    /// Registers ASP.NET Core Identity services for login/credential management.
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Configures Identity services with custom password options and sets up 
        /// the AppDbContext for storing user and role information.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}