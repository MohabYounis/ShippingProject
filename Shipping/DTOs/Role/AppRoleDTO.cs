using Shipping.DTOs.RolePermission;

namespace Shipping.DTOs.Role
{
    public class AppRoleDTO
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        // Role permissions
        public List<RolePermissionDTO> RolePermissions { get; set; } = new List<RolePermissionDTO>();
    }
}
