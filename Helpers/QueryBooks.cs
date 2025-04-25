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
    }
}