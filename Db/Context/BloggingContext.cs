using Microsoft.EntityFrameworkCore;

namespace Db
{
    public partial class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; } = default!;
        public DbSet<Post> Posts { get; set; } = default!;

        #region Constructors and configurations

        private string dbPath { get; }

        public BloggingContext()
        {
            // Create default
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            dbPath = Path.Join(path, "blogging.db");
        }

        public BloggingContext(DbContextOptions<BloggingContext> options, string DbPath)
            : base(options)
        {
            // Store
            dbPath = DbPath;
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Check
            if (!options.IsConfigured) {
                options.UseSqlite($"Data Source={dbPath}");
            }
        }

        #endregion Constructors and configurations

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call for additional definition
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}