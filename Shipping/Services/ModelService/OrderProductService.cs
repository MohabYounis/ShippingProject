using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class OrderProductService : ServiceGeneric<OrderProduct>, IOrderProductService
    {
        public OrderProductService(IRepositoryGeneric<OrderProduct> repository) : base(repository)
        {
        }
    }
}
