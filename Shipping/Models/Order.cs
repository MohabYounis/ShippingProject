using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    public enum OrderType
    {
        PickupFromBranch,       // التسليم في الفرع يعني مش هيكلفني حاجه
        PickupFromMerchant      // استلام الشحنة من التاجر يعني هضيف تكلفة ال pickup الخاصة اللي سجلتها وانا بضيف التاجر
    }
    public enum PaymentTypee
    {
        Prepaid,
        PackageSwap,
        CashOnDelivery
    }
    public enum OrderStatus
    {
        New,                // جديد
        Pendding,           // قيد الانتظار
        AssignedToCourier,  // تم التسليم للمندوب
        InTransit,          // في الطريق
        Delivered,          // تم التسليم للعميل
        Canceled,           // تم الإلغاء
        Returned            // تم الإرجاع
    }

    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("Merchant")]
        public int Merchant_Id { get; set; }
        [ForeignKey("ShippingType")]
        public int ShippingType_Id { get; set; }
        [ForeignKey("WeightPricing")]
        public int WeightPricing_Id { get; set; }
        [ForeignKey("Delivery")]
        public int Delivery_Id { get; set; }
        [ForeignKey("Government")]
        public int Dovernment_Id { get; set; }
        [ForeignKey("City")]
        public int City_Id { get; set; }
        [ForeignKey("ShippingToVillage")]
        public int ShippingToVillage_Id { get; set; }
        public OrderType OrderType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone1 { get; set; }
        public string ClientPhone2 { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public bool DeliverToVillage { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        public PaymentTypee PaymentType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal ShippingCost { get; set; } // depend on weight, additional weight, shipping type city price [standard or pickup], deliver to village, payment type and order type
        public virtual Merchant? Merchant { get; set; }
        public virtual List<OrderProduct>? OrderProducts { get; } = new List<OrderProduct>();
        public virtual ShippingType? ShippingType { get; set; }
        public virtual WeightPricing? WeightPricing { get; set; }
        public virtual Delivery? Delivery { get; set; }
        public virtual Government? Government { get; set; }
        public virtual City? City { get; set; }
        public virtual ShippingToVillage? ShippingToVillage { get; set; }
    }
}
