using Shipping.DTOs.OrderDTOs;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IOrderService : IServiceGeneric<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersByStatus (string orderStatus);
        Task <IEnumerable<Order>> GetAllExistOrdersByStatus (string orderStatus);
        
        Task <IEnumerable<Order>> GetAllByDeliveryByStatus (int id, string orderStatus);
        Task<IEnumerable<Order>> GetAllExistByDeliveryByStatus(int id, string orderStatus);

        Task<IEnumerable<Order>> GetAllByMerchantByStatus(int id, string orderStatus);
        Task<IEnumerable<Order>> GetAllExistByMerchantByStatus(int id, string orderStatus);

        Task<decimal> CalculateShippingCost(OrderCreateDTO createDTO);

    }
}
