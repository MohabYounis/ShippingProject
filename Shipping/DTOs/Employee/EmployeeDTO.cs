using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class EmployeeDTO
    {
        //employee fields
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        // app user fields

        public string userId { get; set; }  
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        //role
        public string RoleId { get; set; }
        public string Role { get; set; }

       
        // branch fields
        public int branchId { get; set; }
        public string BranchName { get; set; }  
        
    }
}
