using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Shipping.DTOs;
namespace Shipping.Services.ModelService
{
    public class DeliveryService : ServiceGeneric<Delivery>, IDeliveryService
    {
        private readonly IUnitOfWork unitOfWork;

        public DeliveryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<IEnumerable<Government>> GetAllGovernmentExist(List<int> governorateIds)
        {
            if (governorateIds == null || !governorateIds.Any())
            {
                return Enumerable.Empty<Government>(); // إرجاع قائمة فارغة إذا لم يتم تقديم أي ID
            }
            var governmentRepository = unitOfWork.GetRepository<Government>();
            var allGovernments = await governmentRepository.GetAllAsync(); //بجيب هنا كل المحافظات من الداتا بيز
            var existingGovernments = allGovernments.Where(g => governorateIds.Contains(g.Id)).ToList(); // هنا بفلترها بحيث تكون ال ID موجودة في القائمة المقدمة
            return existingGovernments;
        }
    }
}
