using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApi.Models
{
    public class BookStatus
    {
        public string Available { get; } = "Available";
        public string Borrowed { get; } = "Borrowed";
        public string Returned { get; } = "Returned";
        public string Damaged { get; } = "Damaged";

        private BookStatus() {}
        public static readonly BookStatus Status = new BookStatus();
    }
}