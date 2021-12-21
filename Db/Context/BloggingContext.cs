using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Db
{
    public partial class BloggingContext : DbContext
    {
        public const string CI_AS = "CI_AS";

        public DbSet<Blog> Blog { get; set; } = default!;
        public DbSet<Post> Post { get; set; } = default!;
        public DbSet<StaticData> StaticData { get; set; } = default!;

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
                var con = new SqliteConnection($"Data Source={dbPath}");
                con.CreateCollation(CI_AS, (x, y) => string.Compare(x, y, ignoreCase: true));
                options.UseSqlite(con);
            }
        }



        #endregion Constructors and configurations

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Blog
            modelBuilder.Entity<Blog>(entity => { 
                entity.ToTable("r_blog");

                entity.Property(b => b.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(b => b.Url)
                      .HasColumnName("url")
                      .HasColumnType("varchar(256)")
                      .HasMaxLength(256)
                      .UseCollation(CI_AS);

                entity.HasKey(b => b.Id)
                      .HasName("PK_R_BLOG");
            });

            // Configure Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("r_post");

                entity.Property(b => b.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(b => b.Title)
                      .HasColumnName("title")
                      .HasColumnType("varchar(256)")
                      .HasMaxLength(256)
                      .UseCollation(CI_AS);

                entity.Property(b => b.Content)
                     .HasColumnName("content")
                     .HasColumnType("text")
                     .UseCollation(CI_AS);

                entity.Property(b => b.BlogId)
                     .HasColumnName("id_blog");

                entity.HasKey(b => b.Id)
                      .HasName("PK_R_POST");

                modelBuilder.Entity<Post>()
                   .HasOne(p => p.Blog)
                   .WithMany(b => b.Posts)
                   .HasForeignKey(p => p.BlogId)
                   .HasConstraintName("FK_R_POST_R_BLOG"); 
            });

            // Configure StaticData
            modelBuilder.Entity<StaticData>(entity => {
                entity.ToTable("r_static_data");

                entity.Property(b => b.Id)
                      .HasColumnName("id")
                      .ValueGeneratedNever();

                entity.Property(b => b.Version)
                      .HasColumnName("version")
                      .HasColumnType("varchar(256)")
                      .HasMaxLength(256)
                      .UseCollation(CI_AS);

                entity.HasKey(b => b.Id)
                      .HasName("PK_R_STATIC_DATA");
            });

            // Call for additional definition
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}