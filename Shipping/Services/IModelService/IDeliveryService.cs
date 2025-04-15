
using Shipping.DTOs.DeliveryDTOs;
using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IDeliveryService: IServiceGeneric<Delivery>
    {
        Task<IEnumerable<Government>> GetAllGovernmentExist(List<int> governorateIds);
        Task<bool> AddDeliveryAsync(DeliveryCreateDTO deliveryDTO);
        Task<IEnumerable<Delivery>> GetAllDeliveryWithGovernmentsAsync();
        Task<bool> UpdateDeliveryAsync(int deliveryId, DeliveryEditDTO deliveryDTO);
        Task<Delivery> GetDeliveryByIdAsync(int deliveryId);
        Task<IEnumerable<Delivery>> GetDeliveryByBranchIdAsync(int branchId);
        Task<IEnumerable<Government>> GetGovernmentByBranchId(int branchId);
    }
}
