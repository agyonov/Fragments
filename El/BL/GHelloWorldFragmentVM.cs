using Db;
using El.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace El.BL
{
    public class GHelloWorldFragmentVM : RootVM
    {
        private readonly IOptions<AppSettings> _Sett;

        public GHelloWorldFragmentVM(BloggingContext efc, IOptions<AppSettings> Sett) : base(efc)
        {
            _Sett = Sett;
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
                          select new Models.Title(b.Id, b.Url ?? string.Empty))
                          .Take(_Sett.Value.MaxQueueRows)
                          .ToList();

            // Do some async work
            await Task.Delay(100);

            // Set that data
            Titles = titles;
        }
    }
}
