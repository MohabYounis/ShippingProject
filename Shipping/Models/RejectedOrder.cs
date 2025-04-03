using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(Order_Id), nameof(RejectReason_Id))]

    public class RejectedOrder
    {
        [ForeignKey("Order")]
        public int Order_Id { get; set; }
        [ForeignKey("RejectReason")]
        public int RejectReason_Id { get; set; }
        DateTime RejectDate { get; set; } = DateTime.Now;
        public virtual Order? Order { get; set; }
        public virtual RejectReason? RejectReason { get; set; }
    }
}
