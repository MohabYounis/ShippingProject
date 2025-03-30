namespace Shipping.Models
{
    public class RejectReason
    {
        public int Id {  get; set; }
        public string Reason { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual List<RejectedOrder>? RejectedOrders { get; set; } = new();
    }
}
