using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Shipping.Services.IModelService;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Services.ModelService
{
    public class ApplicationRoleService : ApplicationRoleRepository, IApplicationRoleService
    {
        public ApplicationRoleService(ShippingContext context):base(context)
        { 
            
        }
    }
}
