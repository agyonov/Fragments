using Android.Content;
using AndroidX.AppCompat.App;

namespace Fragments.Activities
{
    [Activity(Theme = "@style/AppSplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        private CancellationTokenSource _cancellationTokenSource = default!;
        private Bundle? _savedInstanceState = default!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // call parent
            base.OnCreate(savedInstanceState);

            // Create token source
            _cancellationTokenSource = new CancellationTokenSource();
            _savedInstanceState = savedInstanceState;

            // Wait it
            var ct = _cancellationTokenSource.Token;
            _ = Task.Run(async () => await AppStartup(_savedInstanceState, ct), ct);
        }

        protected override void OnDestroy()
        {
            //Cancel
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            // call parent
            base.OnDestroy();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async Task AppStartup(Bundle? savedInstanceState, CancellationToken ct)
        {

            // Cycle
            while (!Startup.IsInited) {
                await Task.Delay(250, ct);
            }

            // Init Xamarin Essentials
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Notify
            if (!ct.IsCancellationRequested) {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Open Main
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                });
            }
        }
    }
}
