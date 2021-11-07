using System;
using ELDEL_EaseeAPI_Library.Config;
using ELDEL_EaseeAPI_Library.Interceptors;
using ELDEL_EaseeAPI_Library.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ELDEL_EaseeAPI_Library.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddEaseeAPILibrary(this IServiceCollection services, IConfiguration config)
        {
            var easeeAPIConfig = config.GetSection(nameof(EaseeAPILibraryConfig)).Get<EaseeAPILibraryConfig>();
            services.AddSingleton<IEaseeAPILibraryConfig>(easeeAPIConfig);

            services.AddTransient<EaseeAPIAuthenticationHeaderDelegatingHandler>();

            // Add Login / Acess and refresh token handler for injecting Bearer <EaseeAPIAcessToken>
            // into each call towards Easee API's baseUrl.
            services.AddHttpClient(nameof(EaseeAPIAuthenticationHeaderDelegatingHandler), c =>
            {
                c.BaseAddress = new Uri($"{easeeAPIConfig.BaseUri}/");
            }).AddHttpMessageHandler<EaseeAPIAuthenticationHeaderDelegatingHandler>();

            // Add internal services
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IEaseeAPIAuthenticationService, EaseeAPIAuthenticationService>();
            services.AddScoped<IEaseeAPIService, EaseeAPIService>();
            services.AddScoped<IEaseeChargerService, EaseeChargersService>();

            return services;
        }
    }
}
