using Android.Views;
using El.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments.Fragments
{
    public class GHelloWorldFragment : GcFragment<GHelloWorldFragmentVM>
    {
        public GHelloWorldFragment() : base(Resource.Layout.g_hello_world_fragment)
        {
        
        }

        public override void OnResume()
        {
            var ha = VM;
            // Call parent
            base.OnResume();
        }
    }
}
