using AndroidX.AppCompat.App;
using El;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments
{
    public class GcActivity<T> : AppCompatActivity where T : class, IRootVN
    {
        // The scope of the activity
        private IServiceScope scope;

        // The service provider for the scope
        protected IServiceProvider ServiceProvider => scope.ServiceProvider;

        /// <summary>
        /// The ViewModel for the activity
        /// </summary>
        protected readonly T VM;

        public GcActivity() : base()
        {
            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            VM = ServiceProvider.GetRequiredService<T>();
        }

        public GcActivity(int ContentLayOutId) : base(ContentLayOutId)
        {
            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            VM = ServiceProvider.GetRequiredService<T>();
        }

        protected override void OnDestroy()
        {
            // Call parent
            base.OnDestroy();

            // Free
            scope.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            // call parent
            base.Dispose(disposing);

            try {
                // further dispose
                scope.Dispose();
            } catch { }
        }
    }
}
