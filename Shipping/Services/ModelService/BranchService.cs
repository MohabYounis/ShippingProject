using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;
namespace Shipping.Services.ModelService
{
    public class BranchService : ServiceGeneric<Branch>, IBranchService
    {
        public BranchService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
