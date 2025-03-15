using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string AppUser_Id { get; set; }
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
