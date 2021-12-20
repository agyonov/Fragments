using El;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments
{
    public class GcFragment<T> : AndroidX.Fragment.App.Fragment where T : class, IRootVN
    {
        // The scope of the activity
        private IServiceScope scope;

        // The service provider for the scope
        protected IServiceProvider ServiceProvider => scope.ServiceProvider;

        /// <summary>
        /// The ViewModel for the activity
        /// </summary>
        protected readonly T VM;

        public GcFragment() : base()
        {
            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            VM = ServiceProvider.GetRequiredService<T>();
        }

        public GcFragment(int ContentLayOutId) : base(ContentLayOutId)
        {
            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            VM = ServiceProvider.GetRequiredService<T>();
        }


        public override void OnDestroy()
        {
            // further dispose
            scope.Dispose();

            // call parent
            base.OnDestroy();
        }

        protected override void Dispose(bool disposing)
        {
            try {
                // further dispose
                scope.Dispose();
            } catch { }

            // call parent
            base.Dispose(disposing);
        }
    }
}
