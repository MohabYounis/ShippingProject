

using Microsoft.EntityFrameworkCore;
using Shipping.DTOs.RejectedReasonsDTOs;
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
                IsDeleted = r.IsDeleted,
                Reason = r.Reason
            }).ToList();
        } 
        

        public async Task<IEnumerable<RejectReasonDTO>> GetAllExistAsync()
        {
            var rejectReasons = await _repository.GetAllExistAsync();
            return rejectReasons.Select(r => new RejectReasonDTO
            {
                Id = r.Id,
                IsDeleted= r.IsDeleted,
                Reason = r.Reason
            }).ToList();
        }


        public async Task<RejectReasonDTO> GetByIdAsync(int id)
        {
            var rejectReason = await _repository.GetByIdAsync(id);
            if (rejectReason == null) throw new Exception("Not Found.");

            return new RejectReasonDTO
            {
                Id = rejectReason.Id,
                IsDeleted = rejectReason.IsDeleted,
                Reason = rejectReason.Reason
            };
        }


        public async Task AddAsync(RejectReasonCreateDTO rejectReasonDto)
        {
            var rejectReason = new RejectReason
            {
                Reason = rejectReasonDto.Reason
            };

            await _repository.AddAsync(rejectReason);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(int id, RejectReasonDTO rejectReasonDto)
        {
            var existingRejectReason = await _repository.GetByIdAsync(id);
            if (existingRejectReason == null) throw new KeyNotFoundException("Reject Reason not found.");

            existingRejectReason.Reason = rejectReasonDto.Reason;
            existingRejectReason.IsDeleted = rejectReasonDto.IsDeleted;

            _repository.Update(existingRejectReason);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            var rejectReason = await _repository.GetByIdAsync(id);
            if (rejectReason == null) throw new KeyNotFoundException("Reject Reason not found.");

            rejectReason.IsDeleted = true;
            _repository.Update(rejectReason);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}