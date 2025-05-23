﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

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
        New,          
        Pending,          
        DeliveredToAgent,  
        Delivered,         
        CanceledByRecipient,         
        PartiallyDelivered,       
        Postponed,
        CannotBeReached,
        RejectedAndNotPaid,
        RejectedWithPartialPayment,
        RejectedWithPayment
    }

    public class Order
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        [ForeignKey("Merchant")]
        public int Merchant_Id { get; set; }
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        [ForeignKey("ShippingType")]
        public int ShippingType_Id { get; set; }
        [ForeignKey("Delivery")]
        public int? Delivery_Id { get; set; } = null;
        [ForeignKey("Government")]
        public int Government_Id { get; set; }
        [ForeignKey("City")]
        public int City_Id { get; set; }
        public OrderType OrderType { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone1 { get; set; }
        public string ClientPhone2 { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public bool DeliverToVillage { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.New;
        [ForeignKey("RejectReason")]
        public int RejectReason_ID { get; set; }
        public PaymentTypee PaymentType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal OrderCost { get; set; } // depend on merchant pricing
        public decimal ShippingCost { get; set; } // depend on weight, additional weight, shipping type city price [standard or pickup], deliver to village, payment type and order type
        public decimal? DeliveryRight { get; set; }
        public decimal? CompanyRight { get; set; }
        public float OrderTotalWeight { get; set; }
        public string? MerchantNotes { get; set; }
        public string? EmployeeNotes { get; set; }
        public string? DeliveryNotes { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual List<Product>? Products { get; set; } = new List<Product>();
        public virtual ShippingType? ShippingType { get; set; }
        public virtual Delivery? Delivery { get; set; }
        public virtual Government? Government { get; set; }
        public virtual City? City { get; set; }
        public virtual List<RejectedOrder>? RejectedOrders { get; set; } = new();
        public virtual Branch? Branch { get; set; }

        [NotMapped]
        public static int Counter { get; set; } = 40_000_000;
        public Order()
        {
            SerialNumber = (Counter++).ToString();
        }
    }
}
