using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(City_Id), nameof(Merchant_Id))]
    public class SpecialShippingRate
    {
        [ForeignKey("City")]
        public int City_Id { get; set; }
        [ForeignKey("Merchant")]
        public int Merchant_Id { get; set; }
        public decimal SpecialPrice { get; set; }
        public bool IsDeleted { get; set; } = false; // refered to status
        public virtual City? City { get; set; }
        public virtual Merchant? Merchant { get; set; }
    }
}
