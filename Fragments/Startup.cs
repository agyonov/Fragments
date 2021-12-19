using Android.Content.Res;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments
{
    internal static class Startup
    {
        // Store some general
        private static IHost? bHost = null;

        public static void Init(AssetManager? asset)
        {
            // Check if correct
            if (asset != null)
            {
                // Create generic host
                var host = new HostBuilder();

                var folder = Environment.SpecialFolder.Personal;
                var path = Environment.GetFolderPath(folder);

                // Set sillly thing here
                host.ConfigureHostConfiguration(con =>
                {
                    con.AddCommandLine(new string[] { $"ContentRoot={path}" });
                });

                // Add app some configuration from JSON data
                using (var setStream = asset.Open("app.settings.json"))
                {
                    host.ConfigureAppConfiguration(con =>
                    {
                        con.AddJsonStream(setStream);
                    });

                    // Build it
                    bHost = host.Build();
                }
            }


        }
    }
}
