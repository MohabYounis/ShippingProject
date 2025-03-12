using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class DeliveryService : ServiceGeneric<Delivery>, IDeliveryService
    {
        public DeliveryService(IRepositoryGeneric<Delivery> repository) : base(repository)
        {
        }
    }
}
