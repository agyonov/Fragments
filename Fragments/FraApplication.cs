using Android.Runtime;

namespace Fragments
{
    [Application(HardwareAccelerated = true)]
    public class FraApplication : Application
    {
        public FraApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {

        }

        public override async void OnCreate()
        {
            // Call parent
            base.OnCreate();

            // Add here some init code
            await Startup.Init(Assets);
        }
    }
}
