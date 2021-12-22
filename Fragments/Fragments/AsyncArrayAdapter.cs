﻿using Android.Views;
using El.BL;
using El.BL.Models;
using Xamarin.Essentials;

namespace Fragments.Fragments
{
    public class AsyncArrayAdapter : ArrayAdapter<Title>
    {
        private readonly GHelloWorldFragmentVM _Vm;
        private readonly AndroidX.Fragment.App.Fragment _Context;


        public AsyncArrayAdapter(AndroidX.Fragment.App.Fragment Context, int Resource, GHelloWorldFragmentVM Vm) : base(Context.Context, Resource)
        {
            // Store for usage
            _Vm = Vm;
            _Context = Context;

            _Vm.PropertyChanged += _Vm_PropertyChanged;

            //Start loading
            _ = Task.Run(async () => await _Vm.GetTitlesFormDbAsync());
        }

        private void _Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Notify
            MainThread.BeginInvokeOnMainThread(() => NotifyDataSetChanged());
        }

        public override long GetItemId(int position)
        {
            return position;
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
    }
}
