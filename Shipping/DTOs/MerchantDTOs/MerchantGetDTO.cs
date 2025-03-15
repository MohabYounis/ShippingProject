using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shipping.Models;

namespace Shipping.DTOs.MerchantDTOs
{
    public class MerchantGetDTO
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string StoreName { get; set; }
        public bool IsDeleted { get; set; }
        public string Government { get; set; }
        public string City { get; set; }
        public decimal PickupCost { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100%")]
        public decimal RejectedOrderPercentage { get; set; }
        //public List<Branch>? Branches { get; set; }
        //public List<SpecialShippingRate>? SpecialShippingRates { get; set; }
    }
}
