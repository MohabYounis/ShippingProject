using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Models
{
    [PrimaryKey(nameof(Merchant_Id),nameof(Branch_Id))]
    public class BranchMerchant
    {
        [ForeignKey("Merchant")]
        public int Merchant_Id { get; set; }
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public virtual Merchant? Merchant { get; set; }
        public virtual Branch? Branch { get; set; }
    }
}
