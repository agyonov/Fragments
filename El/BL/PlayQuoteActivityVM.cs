using Db;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace El.BL
{
    public class PlayQuoteActivityVM : RootRecipientVM
    {
        private const string SELECTED_TITLE_LAST = "PlayQuoteActivityVM.SELECTED_TITLE_LAST";

        private readonly CacheRepository _Cache;

        public PlayQuoteActivityVM(BloggingContext efc, CacheRepository Cache) : base(efc)
        {
            _Cache = Cache;

            // Restore some
            selectedTitle = _Cache.Get<Models.Title>(SELECTED_TITLE_LAST);
        }

        protected override void OnActivated()
        {
            // call parent
            base.OnActivated();

            // Register
            Messenger.Register<PlayQuoteActivityVM, GHelloWorldFragmentVM.CurrentTitleRequestMessage>(this, (r, m) =>
            {
                // Check and send
                if (r.SelectedTitle != null) {
                    m.Reply(r.SelectedTitle);
                }
            });
        }

        protected override void OnDeactivated()
        {
            // call parent
            base.OnDeactivated();

            // Unregister
            Messenger.UnregisterAll(this);
        }

        private Models.Title? selectedTitle = null;
        public Models.Title? SelectedTitle
        {
            get => selectedTitle;
            set {
                // Set into cache
                if (value != null) {
                    _Cache.Add(SELECTED_TITLE_LAST, value);
                } else {
                    _Cache.Remove(SELECTED_TITLE_LAST);
                }

                // Store it
                _ = SetProperty(ref selectedTitle, value);
            }
        }
    }
}
