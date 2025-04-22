using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Models;

namespace BookStoreApi.Mappers
{
    public static class BookMappers
    {
        public static BookDto ToBookDto(this Book bookModel)
        {
            return new BookDto
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Status = bookModel.Status
            };
        }

        public static Book ToBookFromCreateDto(this CreateBookRequestDto bookDto)
        {
            return new Book
            {
                Isbn = bookDto.Isbn,
                Title = bookDto.Title,
                Author = bookDto.Author,
                Status = BookStatus.Status.Available
            };
        }
    }
}