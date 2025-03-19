using Microsoft.EntityFrameworkCore;
using Shipping.Repository;
using Shipping.Services;
using Shipping.UnitOfWorks;

namespace SHIPPING.Services
{
    public class ServiceGeneric<Tentity> : IServiceGeneric<Tentity> where Tentity : class
    {
        protected readonly IUnitOfWork unitOfWork;

        public ServiceGeneric(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        //--------------------------------------
        public virtual async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await unitOfWork.GetRepository<Tentity>().GetAllAsync();
        }


        public virtual async Task<IEnumerable<Tentity>> GetAllExistAsync()
        {
            return await unitOfWork.GetRepository<Tentity>().GetAllExistAsync();
        }

        public async Task<Tentity> GetByIdAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Tentity>().GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            return entity;
        }

        public async Task AddAsync(Tentity entity)
        {
            await unitOfWork.GetRepository<Tentity>().AddAsync(entity);
        }

        public async Task UpdateAsync(Tentity entity)
        {
            await unitOfWork.GetRepository<Tentity>().Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await unitOfWork.GetRepository<Tentity>().GetByIdAsync(id);

            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop == null || !prop.CanWrite) throw new InvalidOperationException("The entity does not support soft deletion.");

            bool isDeleted = (bool)(prop.GetValue(entity) ?? false);
            if (isDeleted) throw new InvalidOperationException($"Entity with ID {id} is already deleted.");

            await unitOfWork.GetRepository<Tentity>().DeleteByID(id);
        }
        public async Task SaveChangesAsync()
        {
            await unitOfWork.SaveChangesAsync();
        }

       
    }
}
