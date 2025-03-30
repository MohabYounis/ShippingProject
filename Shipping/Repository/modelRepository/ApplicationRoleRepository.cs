using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.UnitOfWorks;
using static Dapper.SqlMapper;

namespace Shipping.Repository.modelRepository
{
    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        private readonly IMapper mapper;

        public ShippingContext Context { get; }
        public ApplicationRoleRepository(ShippingContext context , IMapper mapper)
        {
           this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper;
        }

        public async Task AddAsync(ApplicationRoleDTO entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

           var role = mapper.Map<ApplicationRoleDTO, ApplicationRole>(entity);

            await Context.AddAsync(role);
        }

        public async Task Delete(string id)
        {
            var entityObj = await Context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (entityObj == null) throw new ArgumentNullException(nameof(entityObj));
            entityObj.IsDeleted = true;
            Context.SaveChanges();
        }

        public async Task DeleteByID(string id)
        {
            ApplicationRoleDTO entityObj = await GetByIdAsync(id);
            if (entityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            EF.Property<bool>(entityObj, "IsDeleted"); 
            entityObj.GetType().GetProperty("IsDeleted")?.SetValue(entityObj, true); 
            Context.Update(entityObj);
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllAsyncExist()
        {
            var allRoles = await Context.Set<ApplicationRole>()
                     .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                     .ToListAsync();
            return mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleDTO>>(allRoles);
        }

        //

        public async Task<IEnumerable<ApplicationRole>> GetAllAsync()
        {
            return await Context.Set<ApplicationRole>().ToListAsync();
        }

        public async Task<ApplicationRole> GetByIdAsync(string id)
        {
           var role= await Context.Set<ApplicationRole>().FindAsync(id);
            return mapper.Map<ApplicationRole, ApplicationRoleDTO>(role);
        }

        public void Update(ApplicationRoleDTO entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var existingRole = Context.Roles.FirstOrDefault(r => r.Id == entity.Id);
            if (existingRole != null)
            {
                mapper.Map(entity, existingRole);
                Context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Role not found.");
            }
        }

        public async Task<ApplicationRole> GetByNameAsync(string roleName)
        {
            return await Context.Set<ApplicationRole>()
                                .FirstOrDefaultAsync(r => EF.Functions.Like(r.Name, roleName));
        }

        public async Task UpdateById(string id)
        {
            ApplicationRoleDTO tentityObj = await GetByIdAsync(id);
            if (tentityObj == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
            var role = mapper.Map<ApplicationRoleDTO, ApplicationRole>(tentityObj);
            Context.Update(role);
        }

        public async Task SaveDB()
        {
            await Context.SaveChangesAsync();
        }

    }
}
