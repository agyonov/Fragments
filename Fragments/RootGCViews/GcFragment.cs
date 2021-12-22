using El;
using Microsoft.Extensions.DependencyInjection;

namespace Fragments
{
    public class GcFragment<T> : AndroidX.Fragment.App.Fragment where T : class, IRootVN
    {
        // The scope of the activity
        private IServiceScope scope = default!;

        // The service provider for the scope
        protected IServiceProvider ServiceProvider => scope.ServiceProvider;

        /// <summary>
        /// The ViewModel for the activity
        /// </summary>
        private T _VM = default!;
        protected T VM => _VM;

        public GcFragment() : base()
        {


        }

        public GcFragment(int ContentLayOutId) : base(ContentLayOutId)
        {


        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            // call parent
            base.OnCreate(savedInstanceState);

            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            _VM = ServiceProvider.GetRequiredService<T>();
        }

        public override void OnDestroy()
        {
            // call parent
            base.OnDestroy();

            // further dispose
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
