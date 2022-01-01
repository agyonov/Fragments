using Android.Runtime;
using Google.Android.Material.Snackbar;
using Serilog;
using Xamarin.Essentials;

namespace Fragments
{
    [Application(HardwareAccelerated = true)]
    public class FraApplication : Application
    {
        public FraApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {

        }

        public override void OnCreate()
        {
            // Call parent
            base.OnCreate();

            // Init
            _ = Task.Run(() =>
            {
                // Get path
                string? DocumentsPath = null;
                if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState)) {
                    DocumentsPath = BaseContext?.GetExternalFilesDir("DirectoryDocuments")?.AbsolutePath;
                }

                // Attach global exception handlers
                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
                TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
                AndroidEnvironment.UnhandledExceptionRaiser += UnhandledExceptionRaiser;

                // Add here some init code
                if (!Startup.Init(Assets, DocumentsPath)) {
                    Java.Lang.JavaSystem.Exit(0); // Terminate JVM
                }
            });
        }

        #region Exception handling

        private void UnhandledExceptionRaiser(object? sender, RaiseThrowableEventArgs e)
        {
            // Log it
            LogUnhandledException(e.Exception, "AndroidUnhandledException");

            // Try to recover
            e.Handled = true;

            // Get current activity
            var view = Platform.CurrentActivity.Window?.DecorView;
            if (view != null) {
                // Show
                var snak = Snackbar.Make(view,
                    "Системна грешка! Ако грешката продължава, моля, обадете се на Поддръжка.\r\nПодробности за грешката се записват в журнала на приложението.",
                    Snackbar.LengthIndefinite);
                var snackBarView = snak.View;
                var textView = (TextView?)snackBarView.FindViewById(Resource.Id.snackbar_text);
                textView?.SetMaxLines(5);
                textView?.SetTextAppearance(Resource.Style.GenErrorSnack);
                snak.SetAction("Ok", (v) => { snak.Dismiss(); });
                snak.Show();
            }
        }

        private static void TaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            // Log it
            LogUnhandledException(unobservedTaskExceptionEventArgs.Exception, "UnobservedTaskException");

            // Get current activity
            var view = Platform.CurrentActivity.Window?.DecorView;
            if (view != null) {
                // Show
                var snak = Snackbar.Make(view,
                    "Системна грешка! Ако грешката продължава, моля, обадете се на Поддръжка.\r\nПодробности за грешката се записват в журнала на приложението.",
                    Snackbar.LengthIndefinite);
                var snackBarView = snak.View;
                var textView = (TextView?)snackBarView.FindViewById(Resource.Id.snackbar_text);
                textView?.SetMaxLines(5);
                textView?.SetTextAppearance(Resource.Style.GenErrorSnack);
                snak.SetAction("Ok", (v) => { snak.Dismiss(); });
                snak.Show();
            }
        }

        private static void CurrentDomainOnUnhandledException(object? sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            // Log it
            LogUnhandledException(unhandledExceptionEventArgs.ExceptionObject as Exception, "UnhandledException", true);
        }

        internal static void LogUnhandledException(Exception? exception, string message, bool isFatal = false)
        {
            try {
                // Write
                if (isFatal) {
                    Log.Fatal("Message: {0}.\r\n{1}", message, exception?.ToString());
                } else {
                    Log.Error("Message: {0}.\r\n{1}", message, exception?.ToString());
                }
            } catch {
                // I do not care
            }
        }

        #endregion Exception handling
    }
}
