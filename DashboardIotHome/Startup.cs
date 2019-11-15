using System;
using System.Globalization;
using System.IO;
using DashboardIotHome.Repositories;
using DashboardIotHome.Repositories.Netatmo;
using DashboardIotHome.Repositories.PhilipsHue;
using DashboardIotHome.Repositories.Wunderlist;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DashboardIotHome
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            SetupLogger();

            services.Configure<ApplicationOptions>(Configuration);

            services.AddMvcCore();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                options.SupportedUICultures = new[]
                {
                    // supported languages
                    new CultureInfo("en"),
                    new CultureInfo("sv")
                };
            });

            services.AddCors(cors => cors.AddPolicy(
                "AllowedOrigins",
                builder =>
            {
                builder.AllowCredentials()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .SetIsOriginAllowed(IsOriginAllowed);
            }));

            services.AddHostedService<LifetimeDataHostedService>();

            services.AddTransient<IWunderlistRepository, WunderlistRepository>();
            services.AddTransient<IHueRepository, PhilipsHueRepository>();
            services.AddTransient<IHueRegistry, HueRegistry>();
            services.AddTransient<INetatmoRepository, NetatmoRepository>();

            services.AddSingleton<ICredentials, CredentialsProvider>();
            services.AddSingleton<HttpWrapper>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("AllowedOrigins");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}");
            });
        }

        private bool SetupLogger()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "dashboard.log"))
                    .CreateLogger();

                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to setup logger '{exception.Message}'");
            }

            return false;
        }

        private static bool IsOriginAllowed(string host)
        {
            return true;
        }
    }
}
