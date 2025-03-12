using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;

namespace Shipping.Services.ModelService
{
    public class GovernmentService : ServiceGeneric<Government>, IGovernmentService
    {
        public GovernmentService(IRepositoryGeneric<Government> repository) : base(repository)
        {
        }
    }
}
