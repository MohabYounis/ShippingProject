using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class MerchantService : ServiceGeneric<Merchant>, IMerchantService
    {
        public MerchantService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
