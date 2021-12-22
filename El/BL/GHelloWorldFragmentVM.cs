using Db;
using Microsoft.EntityFrameworkCore;

namespace El.BL
{
    public class GHelloWorldFragmentVM : RootVM
    {
        public GHelloWorldFragmentVM(BloggingContext efc) : base(efc)
        {

        }

        private List<Models.Title> titles = new List<Models.Title>();
        public List<Models.Title> Titles
        {
            get => titles;
            set => SetProperty(ref titles, value);
        }

        public async Task GetTitlesFormDbAsync()
        {
            // Run that thing
            var titles = (from b in DB.Blog.AsNoTracking()
                          orderby b.Url
                          select new Models.Title(b.Id, b.Url ?? string.Empty)
                                ).ToList();

            // Do some async work
            await Task.Delay(1000);

            // Set that data
            Titles = titles;
        }
    }
}
