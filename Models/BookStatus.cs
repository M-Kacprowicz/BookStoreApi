using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApi.Models
{
    public class BookStatus
    {
        public string Available { get; } = "available";
        public string Borrowed { get; } = "borrowed";
        public string Returned { get; } = "returned";
        public string Damaged { get; } = "damaged";

        private BookStatus() {}
        public static readonly BookStatus Status = new BookStatus();
    }
}