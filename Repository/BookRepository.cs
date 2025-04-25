using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Data;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Helpers;
using BookStoreApi.Interfaces;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreApi.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _context;
        public BookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateAsync(Book bookModel)
        {
            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            return bookModel;
        }

        public async Task<Book?> DeleteAsync(int id)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookModel == null)
            {
                return null;
            }

            _context.Books.Remove(bookModel);

            await _context.SaveChangesAsync();

            return bookModel;
        }

        public async Task<List<Book>> GetAllAsync(QueryBooks query)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                books = books.Where(b => b.Author.ToLower().Contains(query.Author.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                books = books.Where(b => b.Title.ToLower().Contains(query.Title.ToLower()));
            }

            return await books.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> UpdateBookInfoAsync(int id, UpdateBookRequestDto bookDto)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookModel == null)
            {
                return null;
            }

            if (!bookDto.Author.IsNullOrEmpty() && bookDto.Author != "string")
                bookModel.Author = bookDto.Author;

            if (!bookDto.Title.IsNullOrEmpty() && bookDto.Title != "string")
                bookModel.Title = bookDto.Title;

            await _context.SaveChangesAsync();

            return bookModel;
        }

        public async Task<Book?> UpdateBookStatusAsync(int id, UpdateBookStatusRequestDto bookStatusDto)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (bookModel == null)
            {
                return null;
            }

            string newStatus = bookStatusDto.Status.ToLower();
            string oldStatus = bookModel.Status.ToLower();

            if (newStatus == BookStatus.Status.Available)
            {
                if (oldStatus == BookStatus.Status.Returned || oldStatus == BookStatus.Status.Damaged)
                {
                    bookModel.Status = newStatus;
                }
                else
                {
                    throw new ArgumentException($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Returned} or {BookStatus.Status.Damaged}.");
                }
            }
            else if (newStatus == BookStatus.Status.Borrowed)
            {
                if (oldStatus == BookStatus.Status.Available)
                {
                    bookModel.Status = newStatus;
                }
                else
                {
                    throw new ArgumentException($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Available}.");
                }
            }
            else if (newStatus == BookStatus.Status.Returned)
            {
                if (oldStatus == BookStatus.Status.Borrowed)
                {
                    bookModel.Status = newStatus;
                }
                else
                {
                    throw new ArgumentException($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Borrowed}.");
                }
            }
            else if (newStatus == BookStatus.Status.Damaged)
            {
                if (oldStatus == BookStatus.Status.Available || oldStatus == BookStatus.Status.Returned)
                {
                    bookModel.Status = newStatus;
                }
                else
                {
                    throw new ArgumentException($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Available} or {BookStatus.Status.Returned}.");
                }
            }
            else
            {
                throw new Exception($"Request could not be processed. Possible statuses to be set: {BookStatus.Status.Available}, {BookStatus.Status.Borrowed}, {BookStatus.Status.Damaged}, {BookStatus.Status.Returned}");
            }

            await _context.SaveChangesAsync();

            return bookModel;
        }
    }
}