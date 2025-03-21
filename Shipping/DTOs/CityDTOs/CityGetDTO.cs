using Shipping.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.DTOs.CityDTOs
{
    public class CityGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Governrator")]
        public string GovernmentName { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? PickupShipping { get; set; }
        public decimal? StandardShipping { get; set; }
    }
}
