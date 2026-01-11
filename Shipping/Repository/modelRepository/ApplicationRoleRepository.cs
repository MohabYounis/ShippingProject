using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.UnitOfWorks;
using static Dapper.SqlMapper;

namespace Shipping.Repository.modelRepository
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {

        public ShippingContext Context { get; }
        public ApplicationRoleRepository(ShippingContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(ApplicationRole entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await Context.AddAsync(entity);
        }

        public void Delete(ApplicationRole entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            EF.Property<bool>(entity, "IsDeleted");
            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            Context.Update(entity);
        }

        public async Task DeleteByID(string id)
        {
            ApplicationRole tentityObj = await GetByIdAsync(id);
            if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            EF.Property<bool>(tentityObj, "IsDeleted");
            tentityObj.GetType().GetProperty("IsDeleted")?.SetValue(tentityObj, true);
            Context.Update(tentityObj);
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsyncExist()
        {
            return await Context.Set<ApplicationRole>()
                     .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                     .ToListAsync();
        }

        //

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            return await Context.Set<ApplicationRole>().ToListAsync();
        }

        public async Task<ApplicationRole> GetByIdAsync(string id)
        {
            return await Context.Set<ApplicationRole>().FindAsync(id);
        }

        public void Update(ApplicationRole entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Context.Update(entity);
        }

        public async Task<ApplicationRole> GetByNameAsync(string roleName)
        {
            return await Context.Set<ApplicationRole>()
                                .FirstOrDefaultAsync(r => EF.Functions.Like(r.Name, roleName));
        }

        public async Task UpdateById(string id)
        {
            ApplicationRole tentityObj = await GetByIdAsync(id);
            if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            Context.Update(tentityObj);
        }

        public async Task SaveDB()
        {
            await Context.SaveChangesAsync();
        }

        public Task<IQueryable<ApplicationRole>> GetQueryableRolesAsync(bool includeDeleted)
        {
            IQueryable<ApplicationRole> query = Context.Set<ApplicationRole>();

            if (!includeDeleted)
            {
                query = query.Where(role => !EF.Property<bool>(role, "IsDeleted"));
            }

            return Task.FromResult(query);
        }
    }
}
















