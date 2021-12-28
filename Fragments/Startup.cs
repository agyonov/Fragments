using Android.Content.Res;
using Db;
using El;
using El.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fragments
{
    internal static class Startup
    {
        // Store some general
        private static IHost bHost = default!;
        public static IServiceProvider ServiceProvider => bHost.Services;

        public static void Init(AssetManager? asset)
        {
            // Check if correct
            if (asset != null) {
                // Create generic host
                var host = new HostBuilder();

                // Set some base path
                var folder = Environment.SpecialFolder.Personal;
                var path = Environment.GetFolderPath(folder);

                // Set sillly thing here
                host.ConfigureHostConfiguration(con =>
                {
                    con.AddCommandLine(new string[] { $"ContentRoot={path}" });
                });

                // Add app some configuration from JSON data
                using (var setStream = asset.Open("app.settings.json")) {
                    host.ConfigureAppConfiguration(con =>
                    {
                        con.AddJsonStream(setStream);
                    })
                    .ConfigureServices((hostingContext, services) =>
                    {
                        // Register the IConfiguration instance for General application settings.
                        services.Configure<El.Models.AppSettings>(hostingContext.Configuration.GetSection("AppSettings"));

                        // Configure my service
                        hostingContext.ConfigureContainer(services);
                    });

                    // Build it
                    bHost = host.Build();
                }
            } else {
                throw new Exception("AssetManager is requiered. It must be not null object reference!");
            }

            // Set min threads
            using (var scope = ServiceProvider.CreateScope()) {
                var appSettings = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>();
                ThreadPool.SetMinThreads(appSettings.Value.MinThreads, appSettings.Value.MinIOThreads);
            }

            // Migrate
            using (var scope = ServiceProvider.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<BloggingContext>()) {
                db.Database.Migrate();
            }
        }

        /// <summary>
        /// Configure application specific services
        /// </summary>
        /// <param name="hostingContext">The hosting context</param>
        /// <param name="services">The service collection</param>
        public static void ConfigureContainer(this HostBuilderContext hostingContext, IServiceCollection services)
        {
            // Get some folders
            var folder = Environment.SpecialFolder.Personal;

            // Add Cache
            services.AddDistributedMemoryCache(opt =>
            {
                opt.SizeLimit = 26214400; // 25 Mb
                opt.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
                opt.CompactionPercentage = 0.15;
            });

            // Add library configuration - Library
            var appCfg = hostingContext.Configuration.GetSection("AppSettings").Get<El.Models.AppSettings>();
            services.AddElLibrary(new El.Models.LibraryConfig
            {
                BasePath = Environment.GetFolderPath(folder),
                DataBasePath = Environment.GetFolderPath(folder),
                App = appCfg
            });

            ////Register my session
            //services.AddScoped<ISessionDetails, ServSession>();
        }
    }
}
