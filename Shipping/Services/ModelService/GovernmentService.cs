using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class GovernmentService : ServiceGeneric<Government>, IGovernmentService
    {
        public GovernmentService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
