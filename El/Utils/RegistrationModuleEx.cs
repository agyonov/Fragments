using El.Interfaces;
using El.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace El
{
    public static class RegistrationModuleEx
    {
        // Called to register what id to be registered
        public static void AddElLibrary(this IServiceCollection builder, LibraryConfig cfg)
        {
            // Store
            config = cfg;

            // Register Singleton instances and factories
            builder.AddSingleton<ILibObjectsFactory, LibObjectsFactory>();
            builder.AddSingleton<CacheRepository>();

            // Register other objects
            builder.AddTransient<IRetryService, RetryService>();

            // Register DB Connection & adder special components
            builder.AddTransient(cont => (new LibObjectsFactory(cont)).CreateInstanceDb());

            // Get public non-abstract classes
            var realClasses = from c in typeof(RegistrationModuleEx).Assembly.ExportedTypes
                              where c.IsClass && !c.IsAbstract
                              select c;
            foreach (var c in realClasses) {
                //addapter
                if (c.IsAssignableTo(typeof(DbClassRoot))) {
                    builder.TryAddScoped(c);
                } else if (c.IsAssignableTo(typeof(IRootVN))) {
                    builder.TryAddScoped(c);
                } else if (c.Namespace != null && c.Namespace.StartsWith("El.Nomens")) {
                    builder.TryAddTransient(c);
                } else if (c.Namespace != null && c.Namespace.StartsWith("El.BL")) {
                    builder.TryAddTransient(c);
                }
                //}  else if (c.Namespace != null && c.Namespace.StartsWith("Library.BL.Builders")) {
                //    builder.AddTransient(c);
                //} 
            }

            // Register utilities
            builder.Add(new ServiceDescriptor(
                typeof(Lazy<>), typeof(LazyImpl<>), ServiceLifetime.Transient
            ));
        }

        // Store
        internal static LibraryConfig? config;
    }
}
