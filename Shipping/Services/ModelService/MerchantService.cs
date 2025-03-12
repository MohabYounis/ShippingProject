using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;

namespace Shipping.Services.ModelService
{
    public class MerchantService : ServiceGeneric<Merchant>, IMerchantService
    {
        public MerchantService(IRepositoryGeneric<Merchant> repository) : base(repository)
        {
        }
    }
}
