﻿using Db;
using El.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace El.BL
{
    public class GHelloWorldFragmentVM : RootRecipientVM
    {
        private readonly IOptions<AppSettings> _Sett;

        public GHelloWorldFragmentVM(BloggingContext efc, IOptions<AppSettings> Sett) : base(efc)
        {
            _Sett = Sett;
        }

        protected override void OnActivated()
        {
            // call parent
            base.OnActivated();

            // Register
            Messenger.Register<GHelloWorldFragmentVM, CurrentTitleRequestMessage>(this, (r, m) =>
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
        public List<Models.Title> Titles
        {
            get => titles;
            set => SetProperty(ref titles, value);
        }

        private Models.Title? selectedTitle = null;
        public Models.Title? SelectedTitle
        {
            get => selectedTitle;
            set => SetProperty(ref selectedTitle, value);
        }

        public async Task GetTitlesFormDbAsync()
        {
            // Run that thing
            var titles = (from b in DB.Blog.AsNoTracking()
                          orderby b.Url
                          select new Models.Title(b.Id, b.Url ?? string.Empty))
                          .Take(_Sett.Value.MaxQueueRows)
                          .ToList();

            // Do some async work
            await Task.Delay(100);

            // Set that data
            Titles = titles;
            SelectedTitle = null;
        }

        public class CurrentTitleRequestMessage : RequestMessage<Models.Title>
        {

        }
    }

}
