using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class DeliveryService : ServiceGeneric<Delivery>, IDeliveryService
    {
        public DeliveryService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
