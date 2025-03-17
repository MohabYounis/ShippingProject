using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public enum DiscountType
    {
        Fixed = 1,       // Fixed Discount 
        Percentage = 2,  // Percentage Discount
    }
    public class Delivery
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string AppUser_Id { get; set; }
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        [MaxLength(100)]
        public bool IsDeleted { get; set; } = false;
        //public string GovernmentName { get; set; }
        public DiscountType DiscountType { get; set; }
        [Range(0, 100, ErrorMessage = ("Percentage must be between 0 and 100%"))]
        public decimal CompanyPercentage { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual List<Order>? Orders { get; } = new List<Order>();

        public virtual List<DeliveryGovernment>? DeliveryGovernments { get; } = new List<DeliveryGovernment>();
    }
}
