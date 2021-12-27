using El.BL;
using El.Helpers;
using Fragments.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragments.Activities
{
    [Activity(Label = "PlayQuoteActivity", Theme = "@style/AppTheme.NoActionBar")]
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

            // Get param
            var playId = Intent?.Extras?.GetString("SelectedTitle", null);
            if (playId == null) {
                // Check for saved state
                playId = savedInstanceState?.GetString("SelectedTitle");

                // Check further
                if (playId == null) {
                    Finish();
                    return;
                }
            }

            // Send to ViewModel
            VM.SelectedTitle = playId.DeserializeObject<El.BL.Models.Title>();

            // Get fragment manager
            _ = SupportFragmentManager
                    .BeginTransaction()
                    .SetReorderingAllowed(true)
                    .Replace(Resource.Id.play_quote_fragment_container_view, new PlayQuoteFragment(), null)
                    .Commit();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            // call parent
            base.OnSaveInstanceState(outState);

            // Save selectrd
            outState.PutString("SelectedTitle", VM.SelectedTitle.SerializeObject());
        }
    }
}
