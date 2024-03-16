using EntityFramework.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.UnitOfWork
{
    internal class UnitOfWorkHandler<TContext> : IUowHandler where TContext : DbContext
    {
        private protected readonly DbContext Context;
        private readonly Dictionary<string, IUowRepository> _repositories = [];

        internal UnitOfWorkHandler(IEnumerable<Type> repositoryTypes)
        {
            Context = (DbContext)Activator.CreateInstance(typeof(TContext))
                ?? throw new Exception("Unable to create DbContext");

            _repositories = repositoryTypes
                .ToDictionary(t => t
                                .GetInterfaces()
                                .Where(i => !i.Equals(typeof(IUowRepository)))
                                .FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IUowRepository)))?
                                .Name ?? string.Empty,
                              t => (IUowRepository)(Activator.CreateInstance(t, Context) ?? new object()));
        }

        public T Repository<T>() where T : IUowRepository
        {
            return (T)_repositories[typeof(T).Name];
        }

        public void Dispose()
        {
            DisposeOverrides();
            _repositories.Clear();
            Context.Dispose();
        }

        private protected virtual void DisposeOverrides()
        {
        }
    }
}
