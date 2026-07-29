using System.Collections.Generic;

namespace MyAssignment.Dtos
{
    public class QueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
    }
}
