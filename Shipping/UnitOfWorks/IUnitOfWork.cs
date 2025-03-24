using Shipping.ImodelRepository;
using Shipping.Models;
using Shipping.Repository;

namespace Shipping.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable 
    {
        ShippingContext Context { get; }
        ISpecialShippingRateRepository SpecialShippingRateRepository { get; }
        IRepositoryGeneric<Tentity> GetRepository<Tentity>()   where Tentity : class;
        Task SaveChangesAsync();

    }
}
