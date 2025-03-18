namespace Shipping.DTOs.Branch
{
    public class EditBranchDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }
        public string Location { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
