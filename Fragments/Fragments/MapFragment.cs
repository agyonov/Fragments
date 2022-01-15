using Android.Views;
using El.BL;
using Esri.ArcGISRuntime.UI.Controls;
using Xamarin.Essentials;

namespace Fragments.Fragments
{
    public class MapFragment : GcFragment<MapVM>
    {
        public MapFragment() : base(Resource.Layout.fragment_map)
        {

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            // Call parent
            base.OnCreate(savedInstanceState);

            // Attach
            VM.PropertyChanged += VM_PropertyChanged;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            // call parent
            base.OnViewCreated(view, savedInstanceState);

            // Load some childs
            GetIteams(view);
        }

        public override void OnStart()
        {
            // call parent
            base.OnStart();

            // Create new Map with basemap
            _ = Task.Run(async () => await VM.CreateMapCommand.ExecuteAsync(null));
        }

        private MapView _topMapView = default!;

        private void GetIteams(View rootView)
        {
            // Find recycle view
            _topMapView = rootView.FindViewById(Resource.Id.the_map_at_top_map) as MapView ?? default!;
        }

        private void VM_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) {
                case "WorldMap":
                    // Notify
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Provide used Map to the MapView
                        if (_topMapView != null) {
                            _topMapView.Map = VM.WorldMap;
                        }
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
