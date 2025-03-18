namespace Shipping.DTOs.setting
{
    public class SettingDTO
    {
        public int Id { get; set; }
        public decimal ShippingToVillageCost { get; set; }
        public bool DeliveryAutoAccept { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
