using Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace El.BL
{
    public class PlayQuoteActivityVM : RootRecipientVM
    {
        public PlayQuoteActivityVM(BloggingContext efc) : base(efc)
        {

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
            set => SetProperty(ref selectedTitle, value);
        }
    }
}
