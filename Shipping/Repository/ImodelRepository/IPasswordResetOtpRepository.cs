using Shipping.Models;

namespace Shipping.Repository.ImodelRepository
{
    public interface IPasswordResetOtpRepository
    {
        Task DeactivateAllActiveAsync(string userId, CancellationToken ct = default);

        Task AddAsync(PasswordResetOtp entity, CancellationToken ct = default);

        Task<PasswordResetOtp?> GetActiveAsync(string userId, CancellationToken ct = default);

        Task MarkUsedAsync(int otpId, CancellationToken ct = default);

        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
