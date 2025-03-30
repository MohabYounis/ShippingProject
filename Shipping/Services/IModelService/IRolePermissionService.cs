using Shipping.Enums;
using Shipping.Models;
using SHIPPING.Services;

namespace Shipping.Services.IModelService
{
    public interface IRolePermissionService: IServiceGeneric<RolePermission>
    {
        Task<RolePermission> GetRolePermissin(string role_id, int permission_id);

        Task<AddResult> AddRolePermission(RolePermission rolePermission);

        Task<UpdateREsult> UpdateRolePermissin(RolePermission rolePermission);

        Task<DeleteResult> DeleteRolePermissin(string role_id, int permission_id);
    }
}
