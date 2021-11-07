using System;
using System.Text;
using System.Threading.Tasks;
using ELDEL.Helpers;
using ELDEL_API.Config;
using ELDEL_API.Services;
using ELDEL_EaseeAPI_Library.Config;
using ELDEL_EntityFramework_Library.Config;
using ELDEL_EntityFramework_Library.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;

namespace ELDEL_API.StartupExtensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddWebAPILogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
            });
            services.AddApplicationInsightsTelemetry();
            services.AddApplicationInsightsTelemetryProcessor<ApplicationInsightTelemetryFilterProcessor>();

            return services;
        }

        public static IServiceCollection AddWebAPIAppSettingsConfig(this IServiceCollection services, IConfiguration config)
        {
            var eldelAuthenticationConfig = config.GetSection(nameof(EldelAuthenticationConfig)).Get<EldelAuthenticationConfig>();
            services.AddSingleton<IEldelAuthenticationConfig>(eldelAuthenticationConfig);

            var eldelAPIConfig = config.GetSection(nameof(EldelAPIConfig)).Get<EldelAPIConfig>();
            services.AddSingleton<IEldelAPIConfig>(eldelAPIConfig);

            var entityFrameworkLibraryConfig = config.GetSection(nameof(EldelEntityFrameworkLibraryConfig)).Get<EldelEntityFrameworkLibraryConfig>();
            services.AddSingleton<IEldelEntityFrameworkLibraryConfig>(entityFrameworkLibraryConfig);

            var easeeAPIConfig = config.GetSection(nameof(EaseeAPILibraryConfig)).Get<EaseeAPILibraryConfig>();
            services.AddSingleton<IEaseeAPILibraryConfig>(easeeAPIConfig);

            return services;
        }

        public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IChargersService, ChargersService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAddressService, AddressService>();

            return services;
        }

        public static IServiceCollection AddWebAPIAuthenticationScheme(this IServiceCollection services, IConfiguration config)
        {
            var eldelAuthenticationConfig = config.GetSection(nameof(EldelAuthenticationConfig)).Get<EldelAuthenticationConfig>();

            var key = Encoding.ASCII.GetBytes(eldelAuthenticationConfig.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var accountMachine = context.HttpContext.RequestServices.GetRequiredService<UserManager<Account>>();
                        var account = accountMachine.GetUserAsync(context.HttpContext.User);
                        if (account == null)
                        {
                            context.Fail("Unauthorized, insufficient access rights.");
                        }

                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }

        public static IServiceCollection AddWebAPICookieConfig(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/accounts/sign-in";
                options.AccessDeniedPath = "/accounts/access-denied";
                options.SlidingExpiration = true;
            });

            return services;
        }

        public static IServiceCollection AddWebAPIASPNetCoreIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Account settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}