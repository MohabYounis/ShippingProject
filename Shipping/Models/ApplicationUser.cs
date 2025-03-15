using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual Merchant? Merchant { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Delivery? Delivery { get; set; }
    }
}
