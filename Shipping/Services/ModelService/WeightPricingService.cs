using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class WeightPricingService : ServiceGeneric<WeightPricing>, IWeightPricingService
    {
        public WeightPricingService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
