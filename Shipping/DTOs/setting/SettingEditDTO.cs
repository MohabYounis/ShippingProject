namespace Shipping.DTOs.setting
{
    public class SettingEditDTO
    {  
        public int Id { get; set; }
        public decimal ShippingToVillageCost { get; set; }
        public bool DeliveryAutoAccept { get; set; } = false;
    }
}
