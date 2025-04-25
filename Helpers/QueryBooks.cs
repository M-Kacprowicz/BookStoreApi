using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookStoreApi.Helpers
{
    public class QueryBooks
    {
        public string? Author { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public bool IsPaginated { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}