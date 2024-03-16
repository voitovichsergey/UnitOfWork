using EntityFramework.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.UnitOfWork
{
    public sealed class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly IEnumerable<Type> _repositoryTypes;

        public UnitOfWork()
        {
            _repositoryTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces().Any(i => i.Equals(typeof(IUowRepository))));
        }

        public IUowTransactionHandler StartTransaction()
        {
            return new UnitOfWorkTransactionHandler<T>(_repositoryTypes);
        }

        public IUowHandler Selector()
        {
            return new UnitOfWorkHandler<T>(_repositoryTypes);
        }
    }
}
