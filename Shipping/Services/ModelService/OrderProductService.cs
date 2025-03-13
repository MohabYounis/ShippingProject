using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class OrderProductService : ServiceGeneric<OrderProduct>, IOrderProductService
    {
        public OrderProductService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
