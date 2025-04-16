using Shipping.Models;

namespace Shipping.DTOs.OrderDTOs
{
    public class OrderGetDTO
    {
        public int Id { get; set; }
        public int Branch_Id { get; set; }
        public string SerialNumber { get; set; }
        public string CreatedDate { get; set; }
        public string ClientData { get; set; }
        public string Governrate { get; set; }
        public string City { get; set; }
        public decimal OrderCost { get; set; }
        public string OrderStatus { get; set; }
        public bool IsDeleted { get; set; }
    }
}
