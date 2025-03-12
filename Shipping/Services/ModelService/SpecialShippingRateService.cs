using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class SpecialShippingRateService : ServiceGeneric<SpecialShippingRate>, ISpecialShippingRateService
    {
        public SpecialShippingRateService(IRepositoryGeneric<SpecialShippingRate> repository) : base(repository)
        {
        }
    }
}
