using Android.Views;
using El.BL;

namespace Fragments.Fragments
{
    public class GHelloWorldFragment : GcFragment<GHelloWorldFragmentVM>, AdapterView.IOnItemClickListener
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

            // Set listner
            listView.OnItemClickListener = this;
        }

        public override void OnStart()
        {
            // call parent
            base.OnStart();

            //Start loading
            _ = Task.Run(async () => await VM.GetTitlesFormDbAsync());
        }

        public void OnItemClick(AdapterView? parent, View? view, int position, long id)
        {
            // Set data
            if (VM.Titles.Count > position) {
                VM.SelectedTitle = VM.Titles[position];
            } else {
                VM.SelectedTitle = null;
            }

            // Set it 
            var quoteFrag = new PlayQuoteFragment();

            var ft = ChildFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.playquote_container_view, quoteFrag);
            ft.Commit();
        }
    }
}
