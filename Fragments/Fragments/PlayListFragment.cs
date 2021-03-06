using Android.Content;
using Android.Views;
using El.BL;
using El.BL.Models;
using El.Helpers;
using Fragments.Activities;
using Xamarin.Essentials;

namespace Fragments.Fragments
{
    public class PlayListFragment : GcFragment<PlayListFragmentVM>, AdapterView.IOnItemClickListener
    {

        private readonly CancellationTokenSource _cancellationTokenSource;

        public PlayListFragment() : base(Resource.Layout.g_hello_world_fragment)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        protected override void Dispose(bool disposing)
        {
            // If managed
            if (disposing) {
                _cancellationTokenSource.Dispose();
            }

            // call parent
            base.Dispose(disposing);
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

            // Re-set ancelation token
            _cancellationTokenSource.TryReset();

            //Start loading
            var ct = _cancellationTokenSource.Token;
            _ = Task.Run(async () => await VM.GetTitlesFormDbAsync(ct), ct);
        }

        public override void OnStop()
        {
            // Cancel
            _cancellationTokenSource.Cancel();

            // call parent
            base.OnStop();
        }

        public override void OnDestroy()
        {
            // Clear selection
            VM.SelectedTitle = null;

            // call parent
            base.OnDestroy();
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

            // Check for selection
            if (VM.SelectedTitle != null) {
                // Check if landscape
                if (!showingTwoFragments) {
                    // In-activate
                    VM.IsActive = false;

                    // Show
                    var intent = new Intent(Activity, typeof(PlayQuoteActivity));
                    intent.PutExtra(PlayQuoteActivity.SELECTED_TITLE_INTENT, VM.SelectedTitle.SerializeObject());
                    StartActivity(intent);
                } else {
                    // Set it 
                    _ = ChildFragmentManager.BeginTransaction()
                                .SetReorderingAllowed(true)
                                .Replace(Resource.Id.play_quote_container_view, new PlayQuoteFragment())
                                .Commit();
                }
            } else {
                // Check if landscape
                if (showingTwoFragments) {
                    // Free
                    foreach (var el in ChildFragmentManager.Fragments) {
                        ChildFragmentManager
                            .BeginTransaction()
                            .SetReorderingAllowed(true)
                            .Remove(el)
                            .Commit();
                    }
                }
            }
        }

        public void TrySetSelected()
        {
            // If we have value
            if (VM.SelectedTitle != null) {
                // Check
                var showingTwoFragments = Resources != null
                                            && Resources.Configuration != null
                                            && Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape;

                // Set it 
                if (showingTwoFragments) {
                    _ = ChildFragmentManager.BeginTransaction()
                            .SetReorderingAllowed(true)
                            .Replace(Resource.Id.play_quote_container_view, new PlayQuoteFragment())
                            .Commit();
                }
            }
        }

        #region Adapter class

        public class AsyncArrayAdapter : ArrayAdapter<Title>
        {
            private readonly PlayListFragmentVM _Vm;
            private readonly PlayListFragment _Context;

            public AsyncArrayAdapter(PlayListFragment Context, int Resource, PlayListFragmentVM Vm) : base(Context.Context, Resource)
            {
                // Store for usage
                _Vm = Vm;
                _Context = Context;

                // Attach events
                _Vm.PropertyChanged += TitlesListChanged;
            }

            /// <summary>
            /// Get some counts
            /// </summary>
            public override int Count => _Vm.Titles.Count;

            public override View GetView(int position, View? convertView, ViewGroup parent)
            {
                // re-use an existing view, if one is available
                View? view = convertView;
                // Otherwise create a new one
                if (view == null) {
                    view = _Context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null)!;
                }

                TextView textView = view.FindViewById<TextView>(Android.Resource.Id.Text1)!;
                textView.Text = _Vm.Titles[position].Name;

                return view;
            }

            private void TitlesListChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                switch (e.PropertyName) {
                    case "Titles":
                        // Notify
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            // Reload list
                            NotifyDataSetChanged();

                            // Ping view
                            _Context.TrySetSelected();
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
