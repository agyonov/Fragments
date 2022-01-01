using Android.Views;
using El.BL;
using El.Helpers;
using Fragments.Fragments;

namespace Fragments.Activities
{
    [Activity(Label = "@string/act_paly_qoute", Theme = "@style/AppTheme.NoActionBar", ParentActivity = typeof(MainActivity))]
    public class PlayQuoteActivity : GcActivity<PlayQuoteActivityVM>
    {
        public PlayQuoteActivity() : base()
        {

        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            // call parent
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_play_quote);

            AndroidX.AppCompat.Widget.Toolbar? myToolbar = (AndroidX.AppCompat.Widget.Toolbar?)FindViewById(Resource.Id.act_play_qoute_toolbar);
            if (myToolbar != null) {
                // Attach it as action bar
                SetSupportActionBar(myToolbar);

                // Set back button
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }

            // Get param
            var playId = Intent?.Extras?.GetString("SelectedTitle", null);
            if (playId != null) {
                // Send to ViewModel
                VM.SelectedTitle = playId.DeserializeObject<El.BL.Models.Title>();
            }

            // Get me out
            if (VM.SelectedTitle == null) {
                Finish();
                return;
            }

            // Get fragment manager
            _ = SupportFragmentManager
                    .BeginTransaction()
                    .SetReorderingAllowed(true)
                    .Replace(Resource.Id.play_quote_fragment_container_view, new PlayQuoteFragment(), null)
                    .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {
                case Android.Resource.Id.Home:
                    // Set not active
                    VM.IsActive = false;

                    // Get me out of here
                    OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
