using Shipping.DTOs.GovernmentDTOs;
using Shipping.Models;

namespace Shipping.Services
{
    public interface IGovernmentService
    {
        Task<IEnumerable<GovernmentGetDTO>> GetAllGovernmentsAsync();
        Task<IEnumerable<GovernmentGetDTO>> GetAllExistGovernmentsAsync();
        Task<GovernmentDTO> GetGovernmentByIdAsync(int id);
        Task AddGovernmentAsync(GovernmentCreateDTO governmentCreateDto);
        Task UpdateGovernmentAsync(int id, GovernmentDTO governmentDto);
        Task DeleteGovernmentAsync(int id);
    }
}