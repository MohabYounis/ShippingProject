using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class OrderService : ServiceGeneric<Order>, IOrderService
    {
        public OrderService(IRepositoryGeneric<Order> repository) : base(repository)
        {
        }
    }
}
