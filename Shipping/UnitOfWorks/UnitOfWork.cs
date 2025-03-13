using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository;
using System.Collections.Concurrent;
using static Dapper.SqlMapper;

namespace Shipping.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        
        private bool disposed = false;

        public UnitOfWork(ShippingContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        // Concurrent Dictionary for  lazy intialization of repositories
        private readonly ConcurrentDictionary<Type, Lazy<object>> repositories = new ConcurrentDictionary<Type, Lazy<object>>();

        public ShippingContext Context { get; }

        public IRepositoryGeneric<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IRepositoryGeneric<TEntity>)repositories.GetOrAdd(
                typeof(TEntity),
                entityType => new Lazy<object>(() => new RepositoryGeneric<TEntity>(this))
            ).Value;
        }
        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Context.Dispose();
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
