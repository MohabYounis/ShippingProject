using Shipping.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.RolePermission
{
    public class RolePermissionDTO
    {

        public int Permission_Id { get; set; }
        public string Role_Id { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanAdd { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
      //  public virtual Permission? Permission { get; set; }
      //  public virtual ApplicationRole? Role { get; set; }

    }
}
