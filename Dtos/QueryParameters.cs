namespace MyAssignment.Dtos
{
    /// <summary>
    /// Query parameters accepted by paginated list endpoints.
    /// </summary>
    public class QueryParameters
    {
        private int _pageSize = 10;
        private const int MaxPageSize = 100;

        private int _page = 1;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = false;
    }
}