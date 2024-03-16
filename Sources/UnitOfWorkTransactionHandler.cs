using EntityFramework.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFramework.UnitOfWork
{
    internal sealed class UnitOfWorkTransactionHandler<TContext> : UnitOfWorkHandler<TContext>, IUowTransactionHandler where TContext : DbContext
    {
        private readonly IDbContextTransaction _transaction;
        private bool _commited = false;

        internal UnitOfWorkTransactionHandler(IEnumerable<Type> repositoryTypes) : base(repositoryTypes)
        {
            _transaction = Context.Database.BeginTransaction();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Commit()
        {
            if (Context.ChangeTracker.HasChanges())
            {
                Save();
            }

            _transaction.Commit();
            _commited = true;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (Context.ChangeTracker.HasChanges())
            {
                await SaveAsync(cancellationToken);
            }

            await _transaction.CommitAsync(cancellationToken);
            _commited = await Task.Run(() => { return !cancellationToken.IsCancellationRequested; });
        }

        private protected override void DisposeOverrides()
        {
            if (!_commited)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();
        }
    }
}
