using System.Text.Json.Serialization;

namespace Shipping.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;    // refered to status
        public string Location { get; set; }
        public virtual List<Employee>? Employees { get; } = new List<Employee>();
        public virtual List<Government>? Governments { get; } = new List<Government>();
        public virtual List<BranchMerchant>? BranchMerchants { get; } = new List<BranchMerchant>();
    }
}
