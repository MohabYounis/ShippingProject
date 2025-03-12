using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
namespace Shipping.Services.ModelService
{
    public class BranchService : ServiceGeneric<Branch>, IBranchService
    {
        public BranchService(IRepositoryGeneric<Branch> repository) : base(repository)
        {
        }
    }
}
