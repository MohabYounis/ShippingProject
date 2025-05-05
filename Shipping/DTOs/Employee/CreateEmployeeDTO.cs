using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } 
        public int branchId { get; set; }
    }
}
