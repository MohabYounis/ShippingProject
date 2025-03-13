using Shipping.Models;
using Shipping.Repository.ImodelRepository;
using Shipping.Repository.modelRepository;
using Shipping.UnitOfWorks;

namespace Shipping.Repository.modelRepository
{
    public class PermissionRepository:RepositoryGeneric<Permission> , IPermissionRepository
    {
        public PermissionRepository(UnitOfWork unitOfWork) :base(unitOfWork) { }
        
    }
}
