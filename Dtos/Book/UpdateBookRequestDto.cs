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
        [Length(13, 13, ErrorMessage = "Isbn must be 13 digits number")]
        public ulong Isbn { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}