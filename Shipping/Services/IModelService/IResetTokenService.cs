namespace Shipping.Services.IModelService
{
    public interface IResetTokenService
    {
       string GenerateResetToken(string userId);
    }
}
