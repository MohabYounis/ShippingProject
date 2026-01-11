using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class EmployeeGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Branch_Id { get; set; }
        public string BranchName { get; set; }
        public Dictionary<string, string> Roles { get; set; }
    }
}
