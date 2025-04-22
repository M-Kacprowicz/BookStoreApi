using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Models;

namespace BookStoreApi.Dtos.Book
{
    public class UpdateBookStatusRequestDto
    {
        public string Status { get; set; } = BookStatus.Status.Available;
    }
}