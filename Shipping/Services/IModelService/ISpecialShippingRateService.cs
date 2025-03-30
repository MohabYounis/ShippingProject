using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface ISpecialShippingRateService : IServiceGeneric<SpecialShippingRate>
    {
        Task<SpecialShippingRate> GetSpecialRateByMerchant(int merchantId, int cityId);
    }
}
