
namespace Repository.Helpers
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
    }
}
