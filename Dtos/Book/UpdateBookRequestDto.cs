using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Models;

namespace BookStoreApi.Dtos.Book
{
    public class UpdateBookRequestDto
    {
        [Required]
        [Range(1000000000000, 9999999999999, ErrorMessage = "Isbn must be a 13 digits number")]
        public ulong Isbn { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}