using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public virtual List<RolePermission>? RolePermissions { get; } = new List<RolePermission>();
    }
}
