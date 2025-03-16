using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Shipping.UnitOfWorks;
using SHIPPING.Services;

namespace Shipping.Services.ModelService
{
    public class ApplicationRoleService : ApplicationRoleRepository
    {
        public ApplicationRoleService(ShippingContext context):base(context)
        { 
            
        }
    }
}
