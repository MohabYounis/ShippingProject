using Shipping.Enums;
using Shipping.Models;

namespace Shipping.Repository.ImodelRepository
{
    public interface IRolePermissinRepository: IRepositoryGeneric<RolePermission>
    {
          Task<RolePermission> GetRolePermissin(string role_id, int permission_id);

        Task<AddResult> AddRolePermissin(RolePermission rolePermission);

        Task<UpdateREsult> UpdateRolePermissin(RolePermission rolePermission);
        Task<DeleteResult> DeleteRolePermissin(string role_id, int permission_id);
    }
}
