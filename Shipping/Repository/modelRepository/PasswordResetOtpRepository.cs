using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.UnitOfWorks;

namespace Shipping.Repository.modelRepository
{
    public class PasswordResetOtpRepository:IPasswordResetOtpRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public PasswordResetOtpRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task DeactivateAllActiveAsync(string userId, CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow;

            var actives = await unitOfWork.Context.PasswordResetOtps
                .Where(x => x.UserId == userId && !x.IsUsed && x.ExpiresAt > now)
                .ToListAsync(ct);

            // Safety: حتى لو كان المفروض واحد بس، نقفل أي حاجة موجودة
            foreach (var item in actives)
                item.IsUsed = true;
        }

        public async Task AddAsync(PasswordResetOtp entity, CancellationToken ct = default)
        {
            await unitOfWork.Context.PasswordResetOtps.AddAsync(entity, ct);
        }

        public Task<PasswordResetOtp?> GetActiveAsync(string userId, CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow;

            return unitOfWork.Context.PasswordResetOtps
                .Where(x => x.UserId == userId && !x.IsUsed && x.ExpiresAt > now)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(ct);
        }

        public async Task MarkUsedAsync(int otpId, CancellationToken ct = default)
        {
            var item = await unitOfWork.Context.PasswordResetOtps.FirstOrDefaultAsync(x => x.Id == otpId, ct);
            if (item != null)
                item.IsUsed = true;
        }

        public Task SaveChangesAsync(CancellationToken ct = default)
            => unitOfWork.Context.SaveChangesAsync(ct);
    
    }
}
