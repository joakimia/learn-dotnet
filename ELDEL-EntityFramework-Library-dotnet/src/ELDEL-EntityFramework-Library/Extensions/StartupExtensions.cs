using System;
using ELDEL_EntityFramework_Library.Config;
using ELDEL_EntityFramework_Library.Models;
using ELDEL_EntityFramework_Library.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELDEL_EntityFramework_Library.StartupExtensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddEldelEntityFrameworkLibrary(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var entityFrameworkLibraryConfig = config.GetSection(nameof(EldelEntityFrameworkLibraryConfig)).Get<EldelEntityFrameworkLibraryConfig>();
            services.AddSingleton(entityFrameworkLibraryConfig);

            // Add Eldel DB tables with ASP.NET Core Identity 
            services.AddDbContextPool<EldelContext>(options => options.UseSqlServer(entityFrameworkLibraryConfig.ConnectionString))
                    .AddIdentity<Account, AccountRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<AccountRole>()
                    .AddEntityFrameworkStores<EldelContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IChargerRepository, ChargerRepository>();
            services.AddScoped<IChargerDetailsRepository, ChargerDetailsRepository>();

            return services;
        }
    }
}
