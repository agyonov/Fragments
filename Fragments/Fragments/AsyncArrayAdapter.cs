using Android.Views;
using El.BL;
using El.BL.Models;
using Xamarin.Essentials;

namespace Fragments.Fragments
{
    public class AsyncArrayAdapter : ArrayAdapter<Title>
    {
        private readonly GHelloWorldFragmentVM _Vm;
        private readonly GHelloWorldFragment _Context;

        public AsyncArrayAdapter(GHelloWorldFragment Context, int Resource, GHelloWorldFragmentVM Vm) : base(Context.Context, Resource)
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
                    MainThread.BeginInvokeOnMainThread(() => { 
                        // Reload list
                        NotifyDataSetChanged();
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
