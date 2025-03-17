//using SHIPPING.Services;
//using Shipping.Models;
//using Shipping.Services.IModelService;
//using Shipping.Repository;
//using Shipping.UnitOfWorks;

//namespace Shipping.Services.ModelService
//{
//    public class GovernmentService : ServiceGeneric<Government>, IGovernmentService
//    {
//        public GovernmentService(UnitOfWork unitOfWork) : base(unitOfWork)
//        {
//        }
//    }
//}
using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService

{
    public class GovernmentService : IGovernmentService
    {
        private readonly IRepositoryGeneric<Government> _governmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GovernmentService(IRepositoryGeneric<Government> governmentRepository, IUnitOfWork unitOfWork)
        {
            _governmentRepository = governmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Government>> GetAllGovernmentsAsync()
        {
            return await _governmentRepository.GetAllAsync();
        }

        public async Task<Government> GetGovernmentByIdAsync(int id)
        {
            return await _governmentRepository.GetByIdAsync(id);
        }

        public async Task AddGovernmentAsync(Government government)
        {
            await _governmentRepository.AddAsync(government);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateGovernmentAsync(Government government)
        {
            _governmentRepository.Update(government);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteGovernmentAsync(int id)
        {
            await _governmentRepository.DeleteByID(id);
            _unitOfWork.SaveChangesAsync();
        }
    }
}