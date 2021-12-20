﻿using Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace El.Utils
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
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                if (RegistrationModuleEx.config != null && RegistrationModuleEx.config.App != null) {
                    _connectionString = Path.Join(path, RegistrationModuleEx.config.App.DbNameMain);
                } else {
                    _connectionString = Path.Join(path, "blogging.db");
                }
            }

            // Set it
            dbContxOpt.UseLazyLoadingProxies(true)
                      .ConfigureWarnings(wc =>
                      {
                          wc.Ignore(CoreEventId.DetachedLazyLoadingWarning);
                      })
                      .UseSqlite($"Data Source={_connectionString}", opt =>
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
