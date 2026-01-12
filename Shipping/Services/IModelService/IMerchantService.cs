using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IMerchantService : IServiceGeneric<Merchant>
    {
        public Task<Merchant> GetAllExistByPhoneNumberAsync(string phone);
    }
}
