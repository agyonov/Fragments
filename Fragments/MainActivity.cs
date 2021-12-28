using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Fragments.Fragments;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;

namespace Fragments
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private const string SLECTED_ITEM_ID = "SLECTED_ITEM_ID";
        private int selectedItemId = 0;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // Call Parent
            base.OnCreate(savedInstanceState);

            // Init Xamarin Essentials
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            if (fab != null) fab.Click += FabOnClick;

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            if (drawer != null) drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            if (navigationView != null) {
                navigationView.SetNavigationItemSelectedListener(this);
            }

            // Check whether we're recreating a previously destroyed instance
            if (savedInstanceState != null) {
                // Restore value of members from saved state
                selectedItemId = savedInstanceState.GetInt(SLECTED_ITEM_ID);
            }
        }

        protected override void OnResume()
        {
            // call parent
            base.OnResume();

            // Check and call
            if (selectedItemId > 0) {
                handleItemId();
            }
        }

        protected override void OnPause()
        {
            // Free
            clearItems();

            // call parent
            base.OnPause();
        }

        public override void OnBackPressed()
        {
            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer != null && drawer.IsDrawerOpen(GravityCompat.Start)) {
                drawer.CloseDrawer(GravityCompat.Start);
            } else {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu? menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings) {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object? sender, EventArgs eventArgs)
        {
            if (sender != null) {
                View view = (View)sender;
                Snackbar.Make(view, "Replace with your own action", BaseTransientBottomBar.LengthLong)
                       .SetAction("Action", (view) => { }).Show();
            }
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            // Save it
            selectedItemId = item.ItemId;
            handleItemId();

            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer != null) drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }


        private void handleItemId()
        {
            if (selectedItemId == Resource.Id.nav_camera) {
                // Free
                clearItems();
            } else if (selectedItemId == Resource.Id.nav_gallery) {
                // Get fragment manager
                SupportFragmentManager
                    .BeginTransaction()
                    .SetReorderingAllowed(true)
                    .Replace(Resource.Id.main_fragment_container_view, new GHelloWorldFragment(), null)
                    .Commit();
            } else if (selectedItemId == Resource.Id.nav_slideshow) {

            } else if (selectedItemId == Resource.Id.nav_manage) {

            } else if (selectedItemId == Resource.Id.nav_share) {

            } else if (selectedItemId == Resource.Id.nav_send) {

            }
        }
        private void clearItems() 
        {
            // Free
            foreach (var el in SupportFragmentManager.Fragments) {
                SupportFragmentManager
                    .BeginTransaction()
                    .SetReorderingAllowed(true)
                    .Remove(el)
                    .Commit();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            // Save
            outState.PutInt(SLECTED_ITEM_ID, selectedItemId);

            // call parent
            base.OnSaveInstanceState(outState);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}