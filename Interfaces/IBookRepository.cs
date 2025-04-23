using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Models;

namespace BookStoreApi.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book bookModel);
        Task<Book?> UpdateBookInfoAsync(int id, UpdateBookRequestDto bookDto);
        Task<Book?> UpdateBookStatusAsync(int id, UpdateBookStatusRequestDto bookStatusDto);
        Task<Book?> DeleteAsync(int id);
        Task<List<Book>> GetByAuthorAsync(string author);
    }
}