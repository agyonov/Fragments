using AndroidX.AppCompat.App;
using El;
using Microsoft.Extensions.DependencyInjection;

namespace Fragments
{
    public class GcActivity<T> : AppCompatActivity where T : class, IRootVN
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

        public GcActivity() : base()
        {

        }

        public GcActivity(int ContentLayOutId) : base(ContentLayOutId)
        {

        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // call parent
            base.OnCreate(savedInstanceState);

            // Start new scope
            scope = Startup.ServiceProvider.CreateScope();

            // Create ViewModel
            _VM = ServiceProvider.GetRequiredService<T>();
        }

        protected override void OnDestroy()
        {
            // Free
            scope.Dispose();

            // Call parent
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
