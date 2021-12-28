using Android.OS;
using Android.Util;
using Android.Views;
using El.BL;

namespace Fragments.Fragments
{
    public class PlayQuoteFragment : GcFragment<PlayQuoteVM>
    {
        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Check container
            if (container == null) {
                return null;
            }

            // Check play
            var play = VM.GetPlayQuote();
            if (play == null) {
                return null;
            }


            // Create title
            var titleView = new TextView(Activity);
            var padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 16, Activity.Resources!.DisplayMetrics));
            titleView.SetPadding(padding, padding, padding, padding);
            titleView.TextSize = 24;
            titleView.Text = play.Title;

            // Create text
            var textView = new TextView(Activity);
            padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 24, Activity.Resources!.DisplayMetrics));
            textView.SetPadding(padding, padding, padding, padding);
            textView.TextSize = 20;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) {
                textView.JustificationMode = Android.Text.JustificationMode.InterWord;
            }
            textView.Text = play.Quote;

            var scroller = new ScrollView(Activity);
            scroller.AddView(textView);

            // Create linear view
            var ll = new LinearLayout(Activity)
            {
                Orientation = Orientation.Vertical
            };
            ll.AddView(titleView);
            ll.AddView(scroller);

            return ll;
        }
    }
}
