using SHIPPING.Services;
using Shipping.Models;
using Shipping.Services.IModelService;
using Shipping.Repository;
using Shipping.UnitOfWorks;

namespace Shipping.Services.ModelService
{
    public class ApplicationUserService : ServiceGeneric<ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
