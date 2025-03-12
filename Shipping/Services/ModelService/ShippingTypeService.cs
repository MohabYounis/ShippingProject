using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;

namespace Shipping.Services.ModelService
{
    public class ShippingTypeService : ServiceGeneric<ShippingType>, IShippingTypeService
    {
        public ShippingTypeService(IRepositoryGeneric<ShippingType> repository) : base(repository)
        {
        }
    }
}
