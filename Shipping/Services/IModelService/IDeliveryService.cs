using Shipping.Models;

namespace Shipping.Services.IModelService
{
    public interface IDeliveryService: IServiceGeneric<Delivery>
    {
        Task<IEnumerable<Government>> GetAllGovernmentExist(List<int> governorateIds);
    }
}
