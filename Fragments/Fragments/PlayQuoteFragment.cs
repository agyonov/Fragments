using Android.Util;
using Android.Views;
using El.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Create text
            var textView = new TextView(Activity);
            var padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 16, Activity.Resources!.DisplayMetrics));
            textView.SetPadding(padding, padding, padding, padding);
            textView.TextSize = 24;
            textView.Text = play.Quote;

            var scroller = new ScrollView(Activity);
            scroller.AddView(textView);

            return scroller;
        }
    }
}
