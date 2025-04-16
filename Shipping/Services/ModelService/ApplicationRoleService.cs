using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        //
        private readonly UserManager<ApplicationUser> userManager;
        public ApplicationRoleService(ShippingContext context, UserManager<ApplicationUser> userManager, IRepositoryGeneric<ApplicationRole> roleRepo, IApplicationRoleRepository userRoleRepo, IMemoryCache memoryCache) : base(context, userManager)
        {
            this.roleRepo = roleRepo;
            this.memoryCache = memoryCache;
            this.userRoleRepo = userRoleRepo;
            this.userManager = userManager;
        }

        public void ResetCache()
        {
            memoryCache.Remove(RolesCacheKey);
        }

        #region Get All Roles
        public async Task<IEnumerable<ApplicationRoleDTO>> GetAllAsync()
        {
            var query = await roleRepo.GetAllAsync();

            var result = await query
                .Where(r => !EF.Property<bool>(r, "IsDeleted")) //existing
                .Select(r => new ApplicationRoleDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                })


                .ToListAsync();

            return result;
        }
        #endregion

        #region Get All By Check Cache
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

        #endregion
       
        //

        #region Git Distinct Roles
        public async Task<ApplicationRoleDTO> GetRoleByUserIdAsync(string userId)
        {

            var applicationRole = await userRoleRepo.GetRoleByUserIdAsync(userId);

            if (applicationRole == null)
            {
                return null;
            }

            //cashed dic
            var roleDictionary = await GetRoleDictionaryAsync();

            var roleEntry = roleDictionary.FirstOrDefault(r => r.Value.Equals(applicationRole.Name, StringComparison.OrdinalIgnoreCase));
            if (roleEntry.Key == null)
            {
                return null;
            }

            return new ApplicationRoleDTO
            {
                Id = roleEntry.Key,
                Name = roleEntry.Value
            };
        }
        public async Task<ApplicationRoleDTO> GetByNameAsync(string roleName)
        {
            var roleDictionary = await GetRoleDictionaryAsync();
            var roleEntry = roleDictionary.FirstOrDefault(r => r.Value.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (roleEntry.Key == null)
            {
                return null;
            }

            return new ApplicationRoleDTO
            {
                Id = roleEntry.Key,
                Name = roleEntry.Value
            };
        }
        #endregion
    }


}

