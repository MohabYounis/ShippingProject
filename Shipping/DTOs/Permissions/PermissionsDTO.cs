using Shipping.Models;

namespace Shipping.DTOs.Permissions
{
    public class PermissionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
