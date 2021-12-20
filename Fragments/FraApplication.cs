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

        public override void OnCreate()
        {
            // Call parent
            base.OnCreate();

            // Add here some init code
            Startup.Init(Assets);            ;
        }
    }
}
