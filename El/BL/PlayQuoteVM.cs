using Db;
using El.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace El.BL
{
    public class PlayQuoteVM : RootRecipientVM
    {
        public PlayQuoteVM(BloggingContext efc) : base(efc)
        {

        }

        public PlayQuote? GetPlayQuote()
        {
            // locals
            PlayQuote? res = null;

            // Get it
            var ct = Messenger.Send(new GHelloWorldFragmentVM.CurrentTitleRequestMessage());

            // return
            if (!ct.HasReceivedResponse) {
                return res;
            }

            // Get from database
            res = (from p in DB.Post
                   where p.Id == ct.Response.Id
                   select new PlayQuote(p.Id, p.Title ?? string.Empty, p.Content ?? string.Empty)).FirstOrDefault();

            // return
            return res;
        }
    }
}
