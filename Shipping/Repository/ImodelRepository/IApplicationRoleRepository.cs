using Shipping.DTOs;
using Shipping.Models;
using static Dapper.SqlMapper;

namespace Shipping.Repository.ImodelRepository
{
    public interface IApplicationRoleRepository
    {
        Task<ApplicationRoleDTO> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationRoleDTO>> GetAllAsync();
        Task AddAsync(ApplicationRoleDTO entity);
        Task UpdateById(string id);
        Task DeleteByID(string id);
        void Update(ApplicationRoleDTO entity);
        Task Delete(string id);
        void SaveDB();
    }
}
