using System;
using ELDEL_API.StartupExtensions;
using ELDEL_EaseeAPI_Library.Extensions;
using ELDEL_EntityFramework_Library.StartupExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ELDEL_API
{
    public class Startup
    {
        public Startup()
        {
            var environment = Environment.GetEnvironmentVariable("ELDEL_ENVIRONMENT");

            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"./appsettings.{environment}.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"./local.settings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Adding ELDEL config, logging, authentication scheme and services. Ordering matters here
            services.AddWebAPILogging();
            services.AddWebAPIServices();
            services.AddEldelEntityFrameworkLibrary(Configuration);
            services.AddWebAPIAppSettingsConfig(Configuration);
            services.AddWebAPIASPNetCoreIdentity();
            services.AddWebAPICookieConfig();
            services.AddWebAPIAuthenticationScheme(Configuration);
            services.AddEaseeAPILibrary(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ELDEL API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Add middleware for ASP.NET Core Identity - Ordering matters on this one.
            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "ELDEL API V1");
            });
        }
    }
}
