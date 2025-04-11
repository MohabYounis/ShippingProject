using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Services.ModelService
{
    public class ApplicationRoleService : ApplicationRoleRepository, IApplicationRoleService
    {
        //
         IRepositoryGeneric<ApplicationRole> roleRepo;
        //
        IApplicationRoleRepository userRoleRepo;

        // cache
        private readonly IMemoryCache memoryCache;
        private const string RolesCacheKey = "RolesCacheKey";

        public ApplicationRoleService(ShippingContext context, IRepositoryGeneric<ApplicationRole> roleRepo, IApplicationRoleRepository userRoleRepo, IMemoryCache memoryCache) :base(context)
        {
            this.roleRepo = roleRepo;  
            this.memoryCache = memoryCache;
            this.userRoleRepo = userRoleRepo;
        }
       public async Task<IEnumerable<ApplicationRoleDTO>> GetAllAsync()
        {
            var query = await roleRepo.GetAllAsync();  // تنتظر الـ Task للحصول على IQueryable

            var result = await query
                .Where(r => !EF.Property<bool>(r, "IsDeleted")) 
                .Select(r => new ApplicationRoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                })
            
                
                .ToListAsync();

            return result;
        }


        public async Task<Dictionary<string, string>> GetRoleDictionaryAsync()
        {
            if (memoryCache.TryGetValue(RolesCacheKey, out Dictionary<string, string> cachedRoles))
            {
                return cachedRoles;
            }

            //get roles 
            var roles = await this.GetAllAsync(); 
           // cashing roles in dictionary
            cachedRoles = roles.ToDictionary(r => r.Id, r => r.Name);


            // set cache options and expiration
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

            memoryCache.Set(RolesCacheKey, cachedRoles, cacheEntryOptions);

            return cachedRoles;
        }

        public void ResetCache()
        {
            memoryCache.Remove(RolesCacheKey);
        }

        //
        public async Task<ApplicationRoleDTO> GetRoleByUserIdAsync(string userId)
        {
            var applicationRole = await userRoleRepo.GetRoleByUserIdAsync(userId);

            if (applicationRole == null)
            {
                return null;
            }


            //mapping
            var roleDTO = new ApplicationRoleDTO
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name
            };

            return roleDTO;
        }


    }
}
