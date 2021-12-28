using Android.Content;
using Android.Views;
using El.BL;
using El.Helpers;
using Fragments.Activities;

namespace Fragments.Fragments
{
    public class GHelloWorldFragment : GcFragment<GHelloWorldFragmentVM>, AdapterView.IOnItemClickListener
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        public GHelloWorldFragment() : base(Resource.Layout.g_hello_world_fragment)
        {
            _cancellationTokenSource = new CancellationTokenSource();
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

        public override void OnResume()
        {
            // call parent
            base.OnResume();

            //Start loading
            var ct = _cancellationTokenSource.Token;
            _ = Task.Run(async () => await VM.GetTitlesFormDbAsync(ct), ct);
        }

        public override void OnPause()
        {
            // Cancel
            _cancellationTokenSource.Cancel();

            // call parent
            base.OnPause();
        }

        public void OnItemClick(AdapterView? parent, View? view, int position, long id)
        {
            // Set data
            if (VM.Titles.Count > position) {
                VM.SelectedTitle = VM.Titles[position];
            } else {
                VM.SelectedTitle = null;
            }

            // Check
            var showingTwoFragments = Resources != null
                                        && Resources.Configuration != null
                                        && Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape;

            // Check if landscape
            if (!showingTwoFragments) {
                // In-activate
                VM.IsActive = false;

                // Show
                var intent = new Intent(Activity, typeof(PlayQuoteActivity));
                intent.PutExtra("SelectedTitle", VM.SelectedTitle.SerializeObject());
                StartActivity(intent);
            } else {
                // Set it 
                _ = ChildFragmentManager.BeginTransaction()
                            .SetReorderingAllowed(true)
                            .Replace(Resource.Id.play_quote_container_view, new PlayQuoteFragment())
                            .Commit();
            }

        }
    }
}
