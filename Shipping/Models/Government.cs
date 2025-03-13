using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Government
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual List<City>? Cities { get; } = new List<City>();
        public virtual List<Order>? Orders { get; } = new List<Order>();
    }
}
