using Android.Views;
using El.BL;

namespace Fragments.Fragments
{
    public class ExceptionFragment : GcFragment<ExceptionFragmentVM>
    {
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Check container
            if (container == null) {
                return null;
            }

            // Create linear view
            var ll = new LinearLayout(Activity)
            {
                Orientation = Orientation.Vertical
            };

            return ll;
        }

        public override void OnStart()
        {
            // Call parent
            base.OnStart();

            // Test throw
            throw new Exception("тест неприхваната грешка");
        }
    }
}
