
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.UnitOfWorks;
using static Dapper.SqlMapper;

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
            return await Context.Set<Tentity>()
                        .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                        .ToListAsync(); // Soft Delete
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
            // Context.Remove (tentityObj);
            EF.Property<bool>(tentityObj, "IsDeleted"); // تأكد من وجود الخاصية في الكيان
            tentityObj.GetType().GetProperty("IsDeleted")?.SetValue(tentityObj, true); // تعيين IsDeleted = true
            Context.Update(tentityObj);
        }


        public void Delete(Tentity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            //Context.Remove(entity);
            EF.Property<bool>(entity, "IsDeleted"); // تأكد من وجود الخاصية في الكيان
            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true); // تعيين IsDeleted = true
            Context.Update(entity);
        }

     
      

        public void Update(Tentity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Context.Update(entity);
        }

        public void SaveDB()
        {
            Context.SaveChanges();
        }
    }
}
