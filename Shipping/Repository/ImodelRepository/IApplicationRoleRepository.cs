using Shipping.DTOs;
using Shipping.Models;
using static Dapper.SqlMapper;

namespace Shipping.Repository.ImodelRepository
{
    public interface IApplicationRoleRepository
    {
        Task<ApplicationRole> GetByIdAsync(string id);
        Task<IQueryable<ApplicationRole>> GetQueryableRolesAsync(bool includeDeleted);
        Task<ApplicationRole> GetByNameAsync(string roleName);
        Task<IEnumerable<ApplicationRole>> GetAllAsyncExist();
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task AddAsync(ApplicationRole entity);
        Task UpdateById(string id);
        Task DeleteByID(string id);
        void Update(ApplicationRole entity);
        void Delete(ApplicationRole entity);
        Task SaveDB();

        //
        Task<ApplicationRole> GetRoleByUserIdAsync(string userId);
    }
}
