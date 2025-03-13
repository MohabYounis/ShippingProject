using Shipping.Repository;

namespace Shipping.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable 
    {
        IRepositoryGeneric<Tentity> GetRepository<Tentity>()   where Tentity : class;
       Task SaveChangesAsync();

    }
}
