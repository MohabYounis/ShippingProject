using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Services.ModelService
{
    public class PermissionService : ServiceGeneric<Permission>, IPermissionService
    {
        public PermissionService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
