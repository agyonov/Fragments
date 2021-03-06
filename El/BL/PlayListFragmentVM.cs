using Db;
using El.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace El.BL
{
    public class PlayListFragmentVM : RootRecipientVM
    {
        private const string SELECTED_TITLE_LAST = "GHelloWorldFragmentVM.SELECTED_TITLE_LAST";

        private readonly IOptions<AppSettings> _Sett;
        private readonly CacheRepository _Cache;
        private readonly ILogger _Logger;

        public PlayListFragmentVM(BloggingContext efc, IOptions<AppSettings> Sett,
            CacheRepository Cache, ILogger<PlayListFragmentVM> Logger) : base(efc)
        {
            _Sett = Sett;
            _Cache = Cache;
            _Logger = Logger;
        }

        protected override void OnActivated()
        {
            // call parent
            base.OnActivated();

            // Register
            Messenger.Register<PlayListFragmentVM, CurrentTitleRequestMessage>(this, (r, m) =>
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

        private List<Models.Title> titles = new List<Models.Title>();
        public List<Models.Title> Titles {
            get => titles;
            set => SetProperty(ref titles, value);
        }

        private Models.Title? selectedTitle = null;
        public Models.Title? SelectedTitle {
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

        public async Task GetTitlesFormDbAsync(CancellationToken ct)
        {
            // Run that thing
            var titles = await (from b in DB.Blog.AsNoTracking()
                                orderby b.Url
                                select new Models.Title(b.Id, b.Url ?? string.Empty))
                              .Take(_Sett.Value.MaxQueueRows)
                              .ToListAsync(ct).ConfigureAwait(false);

            // Get from cache
            var cashedTitle = _Cache.Get<Models.Title>(SELECTED_TITLE_LAST);
            if (cashedTitle != null) {
                // Try read
                cashedTitle = titles.FirstOrDefault(t => t.Id == cashedTitle.Id);
            }

            //check 
            if (!ct.IsCancellationRequested) {
                // Set that data
                Titles = titles;
                SelectedTitle = cashedTitle;
            }

            // Log
            _Logger.LogInformation("Received {0} titles from DB.", titles.Count);
        }

        public class CurrentTitleRequestMessage : RequestMessage<Models.Title>
        {

        }
    }

}
