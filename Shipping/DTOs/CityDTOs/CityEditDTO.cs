namespace Shipping.DTOs.CityDTOs
{
    public class CityEditDTO
    {

        public int Government_Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? PickupShipping { get; set; }
        public decimal? StandardShipping { get; set; }
    }
}
