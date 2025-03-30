namespace Shipping.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public decimal ShippingToVillageCost { get; set; }
        public bool DeliveryAutoAccept { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
