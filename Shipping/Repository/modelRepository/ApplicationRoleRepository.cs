using Microsoft.AspNetCore.Identity;
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

        readonly UserManager<ApplicationUser> userManager;
        public ApplicationRoleRepository(ShippingContext context, UserManager<ApplicationUser> userManager)
        {
            this.Context = context;
            this.userManager = userManager;
        }
        public async Task AddAsync(ApplicationRole entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await Context.AddAsync(entity);
        }
        public void Delete(ApplicationRole entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);

            Context.Update(entity);
        }

        public async Task DeleteByID(string id)
        {
            ApplicationRole tentityObj = await GetByIdAsync(id);
            if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");

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
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
            }
        }




        // get role using user id 
        public async Task<ApplicationRole> GetRoleByUserIdAsync(string userId)
        {
            //get user 
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            //get roles
            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                return null;
            }

            var roleName = roles.FirstOrDefault();
            var role = await Context.Roles
                .Where(r => r.Name == roleName && !EF.Property<bool>(r, "IsDeleted"))
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return null;
            }

            return role;
        }

        //public Task<IQueryable<ApplicationRole>> GetQueryableRolesAsync(bool includeDeleted)
        //{
        //    return new Task<IQueryable<ApplicationRole>>;
        //}
    }
}
