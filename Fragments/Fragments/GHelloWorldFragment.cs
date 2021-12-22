using Android.Views;
using El.BL;

namespace Fragments.Fragments
{
    public class GHelloWorldFragment : GcFragment<GHelloWorldFragmentVM>
    {
        public GHelloWorldFragment() : base(Resource.Layout.g_hello_world_fragment)
        {

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            // call parent
            base.OnViewCreated(view, savedInstanceState);

            // Get ListView
            ListView listView = Activity.FindViewById<ListView>(Resource.Id.listViewTitles)!;

            // Attach adapter
            listView.Adapter = new AsyncArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, VM);
        }

        public override void OnStart()
        {
            // call parent
            base.OnStart();

            //Start loading
            _ = Task.Run(async () => await VM.GetTitlesFormDbAsync());
        }
    }
}
