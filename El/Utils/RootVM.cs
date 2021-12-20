using Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Data;

namespace El
{
    public interface IRootVN 
    {
        BloggingContext DB { get; }

        IDbContextTransaction BeginTransaction();

        IDbContextTransaction BeginTransaction(IsolationLevel il);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel il, CancellationToken cancellationToken = default);
    }

    public abstract class RootVM : ObservableObject, IRootVN
    {
        public RootVM(BloggingContext efc) : base()
        {
            _efc = efc;
        }

        #region Variables

        private readonly BloggingContext _efc;

        #endregion

        #region Properties

        /// <summary>
        /// DB context
        /// </summary>
        public BloggingContext DB => _efc;

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

    public abstract class RootRecipientVM : ObservableRecipient, IRootVN
    {
        public RootRecipientVM(BloggingContext efc) : base()
        {
            _efc = efc;
        }

        #region Variables

        private readonly BloggingContext _efc;

        #endregion

        #region Properties

        /// <summary>
        /// DB context
        /// </summary>
        public BloggingContext DB => _efc;

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

    public abstract class RootValidatortVM : ObservableValidator, IRootVN
    {
        public RootValidatortVM(BloggingContext efc) : base()
        {
            _efc = efc;
        }

        #region Variables

        private readonly BloggingContext _efc;

        #endregion

        #region Properties

        /// <summary>
        /// DB context
        /// </summary>
        public BloggingContext DB => _efc;

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
