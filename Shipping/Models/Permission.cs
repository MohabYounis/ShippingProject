namespace Shipping.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<RolePermission>? RolePermissions { get; } = new List<RolePermission>();
    }
}
