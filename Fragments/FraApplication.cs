using Android.Runtime;
using Serilog;

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
            Startup.Init(Assets, DocumentsPath);
        }

        #region Exception handling

        private void UnhandledExceptionRaiser(object? sender, RaiseThrowableEventArgs e)
        {
            // Log it
            LogUnhandledException(e.Exception, "AndroidUnhandledException");

            // Try to recover
            e.Handled = true;
        }

        private static void TaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            // Log it
            LogUnhandledException(unobservedTaskExceptionEventArgs.Exception, "UnobservedTaskException");
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
                // just suppress any error logging exceptions
            }
        }

        #endregion Exception handling
    }
}
