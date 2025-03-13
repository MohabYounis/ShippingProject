using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class ShippingTypeService : ServiceGeneric<ShippingType>, IShippingTypeService
    {
        public ShippingTypeService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
