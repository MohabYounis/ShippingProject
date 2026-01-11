using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
