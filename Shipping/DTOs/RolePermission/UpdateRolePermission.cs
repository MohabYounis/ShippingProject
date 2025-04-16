using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.RolePermission
{
    public class UpdateRolePermission
    {

        public int Permission_Id { get; set; }

        public string Role_Id { get; set; }
        public bool CanView { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanAdd { get; set; } = false;
    }
}
