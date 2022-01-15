using Android.Views;
using AndroidX.RecyclerView.Widget;
using El.BL;
using Google.Android.Material.ProgressIndicator;
using Xamarin.Essentials;


namespace Fragments.Fragments
{
    public class BitcoinRatesFragment : GcFragment<BitcoinRatesVM>
    {


        public BitcoinRatesFragment() : base(Resource.Layout.fragment_bitcoin)
        {

        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Get it
            View? rootView = inflater.Inflate(Resource.Layout.fragment_bitcoin, container, false);
            if (rootView == null) {
                return rootView;
            }

            // Bind child views
            ChildView(rootView);

            // Create LinearLayoutManager
            var lm = new LinearLayoutManager(Activity);
            mRecyclerView.SetLayoutManager(lm);
            mRecyclerView.ScrollToPosition(0);

            // Create adapter
            var adapt = new BitcoinAdapter(this);
            mRecyclerView.SetAdapter(adapt);

            return rootView;
        }

        public override void OnStart()
        {
            // call parent
            base.OnStart();

            // Mark date
            if (dateTextView != null && VM.DT != default) {
                dateTextView.Text = $"{Resources.GetString(Resource.String.bitcoin_text_value_date)} {VM.DT.ToLocalTime():dd.MM.yyyy HH:mm}";
            }

            // Attach
            VM.DownloadRatesCommand.PropertyChanged += DownloadRatesCommand_PropertyChanged;
            VM.PropertyChanged += VM_PropertyChanged;

            // Attach click event
            bRefresh.Click += BRefresh_Click;

            // call initially
            if (VM.DT == default || (DateTimeOffset.Now - VM.DT).TotalMinutes >= 1.2) {
                BRefresh_Click(null, EventArgs.Empty);
            }
        }

        private RecyclerView mRecyclerView = default!;
        private TextView dateTextView = default!;
        private Button bRefresh = default!;
        private CircularProgressIndicator cInd = default!;

        private void ChildView(View rootView)
        {
            // Find recycle view
            mRecyclerView = rootView.FindViewById(Resource.Id.bitcoin_recycle_view_rates) as RecyclerView ?? default!;
            dateTextView = rootView.FindViewById(Resource.Id.bitcoin_text_date) as TextView ?? default!;
            bRefresh = rootView.FindViewById(Resource.Id.bitcoin_button_refresh) as Button ?? default!;
            cInd = rootView.FindViewById(Resource.Id.bitcoin_wait_indcator) as CircularProgressIndicator ?? default!;
        }

        private void BRefresh_Click(object? sender, EventArgs e)
        {
            // Call
            _ = Task.Run(async () => await VM.DownloadRatesCommand.ExecuteAsync(null));
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) {
                case "DT":
                    // Notify
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Mark date
                        if (dateTextView != null) {
                            dateTextView.Text = $"{Resources.GetString(Resource.String.bitcoin_text_value_date)} {VM.DT.ToLocalTime():dd.MM.yyyy HH:mm}";
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        private void DownloadRatesCommand_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) {
                case "IsRunning":
                    // Notify
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Mark ended
                        if (VM.DownloadRatesCommand.IsRunning) {
                            bRefresh.Visibility = ViewStates.Gone;
                            cInd.Visibility = ViewStates.Visible;
                        } else {
                            cInd.Visibility = ViewStates.Gone;
                            bRefresh.Visibility = ViewStates.Visible;
                        }
                    });
                    break;
                default:
                    break;
            }
        }

        #region Adapter class

        /// <summary>
        /// This is binding between view and the ViewModel
        /// </summary>
        public class BitcoinAdapter : RecyclerView.Adapter
        {
            private readonly BitcoinRatesFragment _Context;

            public BitcoinAdapter(BitcoinRatesFragment Context)
            {
                // Store
                _Context = Context;

                // Attach
                _Context.VM.PropertyChanged += VM_PropertyChanged;
            }

            private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                switch (e.PropertyName) {
                    case "Rates":
                        // Notify
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            // Reload list
                            NotifyDataSetChanged();
                        });
                        break;
                    default:
                        break;
                }
            }

            public override int ItemCount => _Context.VM.Rates.Count();

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                // Bind it
                var hold = holder as ViewHolder;

                if (hold != null) {
                    hold.DataText.Text = _Context.VM.Rates[position].Description;
                    hold.DataText2.Text = $"1 XBT = {_Context.VM.Rates[position].Rate} {_Context.VM.Rates[position].Code}";
                }
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                // Create a new view.
                View? v = LayoutInflater.From(parent.Context)?.Inflate(Resource.Layout.fragment_bitcoin_item, parent, false);

                return new ViewHolder(v);
            }

            /// <summary>
            /// Bliah it is useless - this is the row view of the list
            /// </summary>
            public class ViewHolder : RecyclerView.ViewHolder
            {
                private readonly TextView _DataText;
                public TextView DataText => _DataText;
                private readonly TextView _DataText2;
                public TextView DataText2 => _DataText2;

                public ViewHolder(View? itemView) : base(itemView)
                {
                    // Store
                    _DataText = itemView?.FindViewById(Resource.Id.bitcoin_text_item) as TextView ?? default!;
                    _DataText2 = itemView?.FindViewById(Resource.Id.bitcoin_text_item_2) as TextView ?? default!;
                }
            }
        }

        #endregion Adapter class
    }
}
