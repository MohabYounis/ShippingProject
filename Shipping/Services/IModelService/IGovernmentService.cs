//namespace Shipping.Services.IModelService
//{
//    public interface IGovernmentService
//    {
//    }
//}

using Shipping.Models;

namespace Shipping.Services
{
    public interface IGovernmentService
    {
        Task<IEnumerable<Government>> GetAllGovernmentsAsync();
        Task<Government> GetGovernmentByIdAsync(int id);
        Task AddGovernmentAsync(Government government);
        Task UpdateGovernmentAsync(Government government);
        Task DeleteGovernmentAsync(int id);
    }
}