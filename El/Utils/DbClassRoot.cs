using Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace El
{
    /// <summary>
    /// Root class to acces DB
    /// </summary>
    public abstract class DbClassRoot : ClassRoot
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor 
        /// </summary>
        public DbClassRoot(BloggingContext Ent)
        {
            //Set connection
            _ent = Ent;
        }

        protected override void Dispose(bool flag)
        {
            // Managed by DI
        }

        protected override ValueTask DisposeAsync(bool flag)
        {
            // Managed by DI - if needed add 
            return ValueTask.CompletedTask;
        }
        #endregion

        #region Variables

        private readonly BloggingContext _ent;

        #endregion

        #region Properties

        /// <summary>
        /// DB context
        /// </summary>
        public BloggingContext DB => _ent;

        #endregion

        #region Transactions

        public IDbContextTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel il)
        {
            // Return
            return DB.Database.BeginTransaction(il);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel il, CancellationToken cancellationToken = default)
        {
            // Return
            return await DB.Database.BeginTransactionAsync(il, cancellationToken);
        }

        #endregion Transactions
    }
}
