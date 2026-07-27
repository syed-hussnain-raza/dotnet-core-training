using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MyAssignment.Data;
using MyAssignment.Helper;
using MyAssignment.Options;
using MyAssignment.Repositories;
using MyAssignment.Services.Auth;
using MyAssignment.Services.Email;
using MyAssignment.Services.Users;
using System.Text;

namespace MyAssignment.Extensions
{
    public static class ServicesExtension
    {
        private const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

        /// <summary>
        /// Registers all scoped application business services, generic repositories, and mapper profiles.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }

        /// <summary>
        /// Registers ASP.NET Core Identity authentication schemes and password policies.
        /// </summary>
        public static IServiceCollection AddAppIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Models.User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // Now, this allow spaces in usernames
                options.User.AllowedUserNameCharacters = AllowedUserNameCharacters;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Registers JWT token authentication middleware and validation parameters.
        /// </summary>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

            return services;
        }

        /// <summary>
        /// Registers OpenAPI and Swagger definition schemas with JWT authorization support.
        /// </summary>
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                });
            });

            return services;
        }

        /// <summary>
        /// Registers MVC controllers with global model state validation filter and suppresses default invalid model state responses.
        /// </summary>
        public static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelStateFilter>();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        /// <summary>
        /// Registers SQL Server database context via dependency injection using the connection string from configuration.
        /// </summary>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        /// <summary>
        /// Registers strongly-typed options sections from application configuration.
        /// </summary>
        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            services.Configure<UrlSettings>(configuration.GetSection(nameof(UrlSettings)));
            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));

            return services;
        }

        /// <summary>
        /// Registers global fallback authorization policies requiring authenticated users by default.
        /// </summary>
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        /// <summary>
        /// Registers API versioning reporting and MVC integration.
        /// </summary>
        public static IServiceCollection AddApiVersioningConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            }).AddMvc();

            return services;
        }
    }
}
