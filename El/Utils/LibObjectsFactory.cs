using Db;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace El
{
    /// <summary>
    /// The implementation
    /// </summary>
    internal class LibObjectsFactory : Interfaces.ILibObjectsFactory
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly IServiceProvider _Cont;

        /// <summary>
        /// The connection string
        /// </summary>
        private static string _connectionString = string.Empty;

        /// <summary>
        /// Injection constructor
        /// </summary>
        /// <param name="cont">The container</param>
        public LibObjectsFactory(IServiceProvider cont)
        {
            _Cont = cont;
        }

        public BloggingContext CreateInstanceDb()
        {
            // Create options builder
            var dbContxOpt = new DbContextOptionsBuilder<BloggingContext>();

            // Check
            if (string.IsNullOrWhiteSpace(_connectionString)) {
                // See what we have
                if (RegistrationModuleEx.config != null && RegistrationModuleEx.config.App != null) {
                    _connectionString = Path.Join(RegistrationModuleEx.config.DataBasePath, RegistrationModuleEx.config.App.DbNameMain);
                } else {
                    var folder = Environment.SpecialFolder.ApplicationData;
                    var path = Environment.GetFolderPath(folder);
                    _connectionString = Path.Join(path, "blogging.db");
                }
            }

            // Create connection
            var conSqlLite = new SqliteConnection($"Data Source={_connectionString}");
            conSqlLite.CreateCollation(BloggingContext.CI_AS, (x, y) => string.Compare(x, y, ignoreCase: true));

            // Set it
            dbContxOpt.UseLazyLoadingProxies(true)
                      .ConfigureWarnings(wc =>
                      {
                          wc.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                      })
                      .UseSqlite(conSqlLite, opt =>
                      {
                          // Check
                          if (RegistrationModuleEx.config != null && RegistrationModuleEx.config.App != null) {
                              opt.CommandTimeout(RegistrationModuleEx.config.App.DbCmdTimeout)
                                 .MaxBatchSize(RegistrationModuleEx.config.App.EFBatchSize);
                          }
                      });

            // Create
            return new BloggingContext(dbContxOpt.Options, _connectionString);
        }

        ///// <summary>
        ///// Create it
        ///// </summary>
        //public SecurityTool CreateInstanceSecurityTool()
        //{
        //    return ActivatorUtilities.CreateInstance<SecurityTool>(_Cont);
        //}
    }
}
