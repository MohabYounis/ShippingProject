using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Shipping.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
        public string? ProfileImagePath { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public virtual Merchant? Merchant { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Delivery? Delivery { get; set; }

        //
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }

    }


}
