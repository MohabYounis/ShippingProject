using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public List<string> Roles_Id { get; set; } = new ();
        [Required]
        public int Branch_Id { get; set; }
    }
}
