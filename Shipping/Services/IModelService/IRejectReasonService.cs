

using Shipping.DTOs;

namespace Shipping.Services.IModelService
{
    public interface IRejectReasonService
    {
        Task<IEnumerable<RejectReasonDTO>> GetAllAsync();
        Task<RejectReasonDTO> GetByIdAsync(int id);
        Task AddAsync(RejectReasonDTO rejectReasonDto);
        Task UpdateAsync(int id, RejectReasonDTO rejectReasonDto);
        Task DeleteAsync(int id);
    }
}