

using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
using Shipping.Models;
using Shipping.Repository;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class RejectReasonService : IRejectReasonService
    {
        private readonly IRepositoryGeneric<RejectReason> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectReasonService(IRepositoryGeneric<RejectReason> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RejectReasonDTO>> GetAllAsync()
        {
            var rejectReasons = await _repository.GetAllAsync();
            return rejectReasons.Select(r => new RejectReasonDTO
            {
                Id = r.Id,
                Reason = r.Reason
            }).ToList();
        }

        public async Task<RejectReasonDTO> GetByIdAsync(int id)
        {
            var rejectReason = await _repository.GetByIdAsync(id);
            if (rejectReason == null) return null;

            return new RejectReasonDTO
            {
                Id = rejectReason.Id,
                Reason = rejectReason.Reason
            };
        }

        public async Task AddAsync(RejectReasonDTO rejectReasonDto)
        {
            var rejectReason = new RejectReason
            {
                Reason = rejectReasonDto.Reason
            };

            await _repository.AddAsync(rejectReason);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, RejectReasonDTO rejectReasonDto)
        {
            var existingRejectReason = await _repository.GetByIdAsync(id);
            if (existingRejectReason == null) throw new KeyNotFoundException("Reject Reason not found.");

            existingRejectReason.Reason = rejectReasonDto.Reason;

            _repository.Update(existingRejectReason);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rejectReason = await _repository.GetByIdAsync(id);
            if (rejectReason == null) throw new KeyNotFoundException("Reject Reason not found.");

            rejectReason.IsDeleted = true;
            _repository.Update(rejectReason);
            _unitOfWork.SaveChangesAsync();
        }
    }
}