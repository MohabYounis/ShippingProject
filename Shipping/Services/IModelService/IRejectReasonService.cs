using Shipping.DTOs.RejectedReasonsDTOs;

namespace Shipping.Services.IModelService
{
    public interface IRejectReasonService
    {
        Task<IEnumerable<RejectReasonDTO>> GetAllAsync();
        Task<IEnumerable<RejectReasonDTO>> GetAllExistAsync();
        Task<RejectReasonDTO> GetByIdAsync(int id);
        Task AddAsync(RejectReasonCreateDTO rejectReasonDto);
        Task UpdateAsync(int id, RejectReasonDTO rejectReasonDto);
        Task DeleteAsync(int id);
    }
}