namespace Shipping.DTOs.pagination
{
    public class GenericPagination<T> where T : class
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
