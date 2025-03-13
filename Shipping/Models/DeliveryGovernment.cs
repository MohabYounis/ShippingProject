using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(Delivery_Id), nameof(Government_Id))]
    public class DeliveryGovernment
    {
        [ForeignKey("Delivery")]
        public int Delivery_Id { get; set; }
        [ForeignKey("Government")]
        public int Government_Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Delivery? Delivery { get; set; }
        public virtual Government? Government { get; set; }
    }
}
