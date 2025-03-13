using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class SpecialShippingRateService : ServiceGeneric<SpecialShippingRate>, ISpecialShippingRateService
    {
        public SpecialShippingRateService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
