namespace Shipping.DTOs.pagination
{
    public class GenericPagination<T> where T : class
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
