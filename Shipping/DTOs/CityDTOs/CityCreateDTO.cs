namespace Shipping.DTOs.CityDTOs
{
    public class CityCreateDTO
    {
        public int Government_Id { get; set; }
        public string Name { get; set; }
        public decimal? PickupShipping { get; set; }
        public decimal? StandardShipping { get; set; }
    }
}
