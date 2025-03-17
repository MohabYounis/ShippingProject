using Shipping.Models;
using static Dapper.SqlMapper;

namespace Shipping.Repository.ImodelRepository
{
    public interface IApplicationRoleRepository
    {
        Task<ApplicationRole> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task AddAsync(ApplicationRole entity);
        Task UpdateById(string id);
        Task DeleteByID(string id);
        void Update(ApplicationRole entity);
        void Delete(ApplicationRole entity);
        void SaveDB();
    }
}
