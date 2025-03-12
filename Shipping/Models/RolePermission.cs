using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(Permission_Id), nameof(Role_Id))]
    public class RolePermission
    {
        [ForeignKey("Permission")]
        public int Permission_Id { get; set; }
        [ForeignKey("Role")]
        public string Role_Id { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public virtual Permission? Permission { get; set; }
        public virtual ApplicationRole? Role { get; set; }
    }
}
