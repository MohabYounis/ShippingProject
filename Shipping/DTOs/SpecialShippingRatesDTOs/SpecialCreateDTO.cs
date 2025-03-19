using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.SpecialShippingRatesDTOs
{
    public class SpecialCreateDTO
    {
        public int City_Id { get; set; }
        public decimal SpecialPrice { get; set; }
    }
}
