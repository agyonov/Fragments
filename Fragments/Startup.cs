using Android.Content.Res;
using Db;
using El;
using El.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace Fragments
{
    internal static class Startup
    {
        // Store some general
        private static IHost bHost = default!;
        internal static IServiceProvider ServiceProvider => bHost.Services;
        private static bool _isInited = false;
        internal static bool IsInited => _isInited;

        internal static bool Init(AssetManager? asset, string? docsPath)
        {
            // Set some base path
            var folder = Environment.SpecialFolder.Personal;
            var path = Environment.GetFolderPath(folder);

            // Check if in debug
            bool isDebugMode = false;
#if DEBUG
            isDebugMode = true;
#endif

            // Create generic host
            var host = new HostBuilder();

            // Create logger
            if (isDebugMode) {
                Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .WriteTo.File(Path.Join(string.IsNullOrWhiteSpace(docsPath) ? path : docsPath, "log_app_.txt"),
                                        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 6291456,
                                        retainedFileCountLimit: 5, retainedFileTimeLimit: TimeSpan.FromDays(5))
                        .CreateLogger();
            } else {
                Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Warning()
                        .Enrich.FromLogContext()
                        .WriteTo.File(Path.Join(string.IsNullOrWhiteSpace(docsPath) ? path : docsPath, "log_app_.txt"),
                                        rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 6291456,
                                        retainedFileCountLimit: 5, retainedFileTimeLimit: TimeSpan.FromDays(5))
                        .CreateLogger();
            }
            host.UseSerilog();

            // Set sillly thing here
            host.ConfigureHostConfiguration(con =>
            {
                con.AddCommandLine(new string[] { $"ContentRoot={path}" });
            });

            // Check if correct
            if (asset != null) {
                // Add app some configuration from JSON data
                using (var setStream = asset.Open("app.settings.json")) {
                    host.ConfigureAppConfiguration(con =>
                    {
                        con.AddJsonStream(setStream);
                    })
                    .ConfigureServices((hostingContext, services) =>
                    {
                        // Register the IConfiguration instance for General application settings.
                        services.Configure<AppSettings>(hostingContext.Configuration.GetSection("AppSettings"));

                        // Configure my service
                        hostingContext.ConfigureContainer(services);
                    });

                    // Build it
                    bHost = host.Build();
                }

                // Set min threads
                using (var scope = ServiceProvider.CreateScope()) {
                    // Set men threads
                    var appSettings = scope.ServiceProvider.GetRequiredService<IOptions<AppSettings>>();
                    ThreadPool.SetMinThreads(appSettings.Value.MinThreads, appSettings.Value.MinIOThreads);

                    //Set Esri key
                    Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = appSettings.Value.EsriKey;
                }

                // Migrate
                using (var scope = ServiceProvider.CreateScope())
                using (var db = scope.ServiceProvider.GetRequiredService<BloggingContext>()) {
                    db.Database.Migrate();
                }

                //return 
                _isInited = true;
                return _isInited;
            } else {
                // Build it
                bHost = host.Build();

                // Get me out of here
                Log.Fatal("AssetManager is requiered. It must be not null object reference!");

                //return 
                return _isInited;
            }
        }

        /// <summary>
        /// Configure application specific services
        /// </summary>
        /// <param name="hostingContext">The hosting context</param>
        /// <param name="services">The service collection</param>
        private static void ConfigureContainer(this HostBuilderContext hostingContext, IServiceCollection services)
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
            var appCfg = hostingContext.Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddElLibrary(new LibraryConfig
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
