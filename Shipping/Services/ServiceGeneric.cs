using Shipping.Repository;
using Shipping.Services;
using Shipping.UnitOfWorks;

namespace SHIPPING.Services
{
    public class ServiceGeneric<Tentity> : IServiceGeneric<Tentity> where Tentity : class
    {
        private readonly IUnitOfWork unitOfWork;

        public ServiceGeneric(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await unitOfWork.GetRepository<Tentity>().GetAllAsync();
        }

        public async Task<Tentity> GetByIdAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Tentity>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            return entity;
        }

        public async Task AddAsync(Tentity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await unitOfWork.GetRepository<Tentity>().AddAsync(entity);
        }

        public async Task UpdateAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Tentity>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            await unitOfWork.GetRepository<Tentity>().UpdateById(id);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Tentity>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            await unitOfWork.GetRepository<Tentity>().DeleteByID(id);
        }
        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}
