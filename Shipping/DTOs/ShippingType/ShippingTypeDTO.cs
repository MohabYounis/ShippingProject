using Shipping.Models;

namespace Shipping.DTOs.ShippingType
{
    public class ShippingTypeDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
