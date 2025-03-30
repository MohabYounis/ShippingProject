
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.UnitOfWorks;
using static Dapper.SqlMapper;

namespace Shipping.Repository
{
    public class RepositoryGeneric<Tentity> : IRepositoryGeneric<Tentity> where Tentity : class
    {
        protected readonly IUnitOfWork unitOfWork;

        public RepositoryGeneric(IUnitOfWork unitOfWork)
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


        public Task<IQueryable<Tentity>> GetAllExistAsync()
        {
            
            return Task.FromResult(Context.Set<Tentity>().AsQueryable()
                .Where(e=>!EF.Property<bool>(e, "IsDeleted")));
         
        }

        public Task<IQueryable<Tentity>> GetAllAsync()
        {
            return Task.FromResult(Context.Set<Tentity>().AsQueryable());
        }

        public async Task AddAsync(Tentity entity)
        {


            await Context.AddAsync(entity);

        }
        // will be delted
        public async Task UpdateById(int id)
        {
            Tentity tentityObj = await GetByIdAsync(id);
            Context.Update(tentityObj);
        }

        public async Task DeleteByID(int id)
        {
            Tentity tentityObj = await GetByIdAsync(id);
            var prop = tentityObj.GetType().GetProperty("IsDeleted");

            prop.SetValue(tentityObj, true);
            Context.Update(tentityObj);
        }

        public void Delete(Tentity entity)
        {
            if (entity == null);
            var prop = entity.GetType().GetProperty("IsDeleted");
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(entity, true);
            }
            Context.Update(entity);
        }

        public async Task Update(Tentity entity)
        {
            Context.Update(entity);
        }

        public async Task<Tentity> GetByNameAsync(string name)
        {
            var entityType = typeof(Tentity);
            var nameProperty = entityType.GetProperty("Name");
            if (nameProperty == null) return null;
            return await Context.Set<Tentity>().FirstOrDefaultAsync(t => EF.Property<string>(t, "Name") == name);
        }
    }
}
