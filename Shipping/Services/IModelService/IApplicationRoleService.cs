using Shipping.DTOs;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;

namespace Shipping.Services.IModelService
{
    public interface IApplicationRoleService : IApplicationRoleRepository
    {
        Task<IEnumerable<ApplicationRoleDTO>> GetAllAsync();

        Task<Dictionary<string, string>> GetRoleDictionaryAsync();

        public void ResetCache();





    }
}
