
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.UnitOfWorks;

namespace Shipping.Repository
{
    public class RepositoryGeneric<Tentity> : IRepositoryGeneric<Tentity> where Tentity : class
    {
        readonly UnitOfWork unitOfWork;

        public RepositoryGeneric(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            if (this.unitOfWork.Context == null)
                throw new InvalidOperationException("UnitOfWork Context cannot be null.");
        }

        protected ShippingContext Context => unitOfWork.Context;
        public async  Task<Tentity> GetByIdAsync(int id)
        {
             return await Context.Set<Tentity>().FindAsync(id);
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await Context.Set<Tentity>().ToListAsync();
        }

        public async Task AddAsync(Tentity entity)
        {

            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await Context.AddAsync(entity);

        }




        public async Task UpdateById(int id)
        {
            Tentity tentityObj = await GetByIdAsync(id);
           if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            Context.Update(tentityObj);
        }

        public async Task DeleteByID(int id)
        {
            Tentity tentityObj = await GetByIdAsync(id);
            if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            Context.Remove (tentityObj);
        }


        public void Delete(Tentity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Context.Remove(entity);
        }

     
      

        public void Update(Tentity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Context.Update(entity);
        }

    }
}
