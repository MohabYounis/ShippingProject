using Shipping.DTOs.DeliveryDTOs;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IDeliveryService: IServiceGeneric<Delivery>
    {
        Task<IEnumerable<Government>> GetAllGovernmentExist(List<int> governorateIds);
        Task<bool> AddDeliveryAsync(DeliveryDTO deliveryDTO);
        Task<IEnumerable<Delivery>> GetAllDeliveryWithGovernmentsAsync();
        Task<bool> UpdateDeliveryAsync(int deliveryId, DeliveryDTO deliveryDTO);
        Task<Delivery> GetDeliveryByIdAsync(int deliveryId);
    }
}
