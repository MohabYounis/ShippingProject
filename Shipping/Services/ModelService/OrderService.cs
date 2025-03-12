using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class OrderService : ServiceGeneric<Order>, IOrderService
    {
        public OrderService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
