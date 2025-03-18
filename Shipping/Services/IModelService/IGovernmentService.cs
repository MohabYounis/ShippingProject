//namespace Shipping.Services.IModelService
//{
//    public interface IGovernmentService
//    {
//    }
//}

using Shipping.DTOs;
using Shipping.Models;

namespace Shipping.Services
{
    public interface IGovernmentService
    {
        

        /////////////////////////
        ///
        Task<IEnumerable<GovernmentDTO>> GetAllGovernmentsAsync();
        Task<GovernmentDTO> GetGovernmentByIdAsync(int id);
        Task AddGovernmentAsync(GovernmentDTO governmentDto);
        Task UpdateGovernmentAsync(int id, GovernmentDTO governmentDto);
        Task DeleteGovernmentAsync(int id);
    }
}