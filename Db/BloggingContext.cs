using Microsoft.EntityFrameworkCore;

namespace Db
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; } = default!;
        public DbSet<Post> Posts { get; set; } = default!;

        public string DbPath { get; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "blogging.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Blog
    {
        public int Id { get; set; }
        public string? Url { get; set; }

        public List<Post> Posts { get; } = default!;
    }

    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; } = default!;
    }
}