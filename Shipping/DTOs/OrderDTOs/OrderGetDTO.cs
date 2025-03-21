namespace Shipping.DTOs.OrderDTOs
{
    public class OrderGetDTO
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string CreatedDate { get; set; }
        public string ClientData { get; set; }
        public string Governrate { get; set; }
        public string City { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
