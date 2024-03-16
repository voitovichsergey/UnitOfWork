namespace EntityFramework.UnitOfWork.Interfaces
{
    public interface IUowTransactionHandler : IUowHandler
    {
        /// <summary>
        /// Intermediate saving changes.
        /// </summary>
        public void Save();

        /// <summary>
        /// Intermediate saving changes.
        /// </summary>
        public Task SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit the transaction.
        /// If the transaction has not been commited, a rollback will be performed.
        /// </summary>
        public void Commit();

        /// <summary>
        /// Commit the transaction.
        /// If the transaction has not been commited, a rollback will be performed.
        /// </summary>
        public Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
