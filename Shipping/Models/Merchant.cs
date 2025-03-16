using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string AppUser_Id { get; set; }
        [MaxLength(100)]
        public bool IsDeleted { get; set; } = false;
        public string StoreName { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage=("Percentage must be between 0 and 100%"))]
        public decimal RejectedOrderPercentage  { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<SpecialShippingRate>? SpecialShippingRates { get; set; }
        public virtual List<Order>? Orders { get; set; }
        public virtual List<BranchMerchant>? BranchMerchants { get; } = new List<BranchMerchant>();
    }
}
