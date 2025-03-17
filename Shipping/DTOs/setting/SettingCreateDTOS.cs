namespace Shipping.DTOs.setting
{
    public class SettingCreateDTOS
    {  
        public decimal ShippingToVillageCost { get; set; }
        public bool DeliveryAutoAccept { get; set; } = false;
    }
}
