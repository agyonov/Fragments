using Db;
using El.BL.Models;
using Microsoft.Toolkit.Mvvm.Messaging;

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
            var ct = Messenger.Send<PlayListFragmentVM.CurrentTitleRequestMessage>();

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
