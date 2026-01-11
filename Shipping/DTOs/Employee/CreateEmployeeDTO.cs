using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        // app user fields
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(?:\+20|0)?1[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number format.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        // branch fields
        //[Required(ErrorMessage = "Branch ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be greater than 0.")]
        public int branchId { get; set; }
        // role 
        // public string RoleId { get; set; }

    }
}
