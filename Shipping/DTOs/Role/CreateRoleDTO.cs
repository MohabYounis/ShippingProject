namespace Shipping.DTOs.Role
{
    public class CreateRoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        //normalized
        public string NormalizedName => Name.ToUpper();
    }
}
