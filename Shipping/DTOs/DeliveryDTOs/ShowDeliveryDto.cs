using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.DeliveryDTOs
{
    public class ShowDeliveryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public List<string> GovernmentName { get; set; }
        public string DiscountType { get; set; }                    //added by Mohab
        public decimal CompanyPercentage { get; set; }              //added by Mohab
        public bool IsDeleted { get; set; } = false;
    }
}
