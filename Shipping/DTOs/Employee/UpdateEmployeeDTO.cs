using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class UpdateEmployeeDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<string> Roles_Id { get; set; }
        public bool IsDeleted { get; set; }
        public int Branch_Id { get; set; }
    }
}
