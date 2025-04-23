using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Data;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Mappers;
using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public BookController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var books = await _context.Books.ToListAsync();

            var bookDto = books.Select(s => s.ToBookDto());

            return Ok(books);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookRequestDto bookDto)
        {
            var bookModel = bookDto.ToBookFromCreateDto();
            await _context.Books.AddAsync(bookModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByIdAsync), new { id = bookModel.Id }, bookModel);
        }

        [HttpPut]
        [Route("updateBookInfo/{bookId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int bookId, [FromBody] UpdateBookRequestDto updateDto)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (bookModel == null)
            {
                return NotFound();
            }

            bookModel.Author = updateDto.Author;
            bookModel.Title = updateDto.Title;
            bookModel.Isbn = updateDto.Isbn;

            await _context.SaveChangesAsync();

            return Ok(bookModel);
        }

        [HttpPut]
        [Route("updateBookStatus/{bookId}")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] int bookId, [FromBody] UpdateBookStatusRequestDto updateStatusDto)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (bookModel == null)
            {
                return NotFound();
            }

            string newStatus = updateStatusDto.Status.ToLower();
            string oldStatus = bookModel.Status.ToLower();

            if (newStatus == BookStatus.Status.Available)
            {
                if (oldStatus == BookStatus.Status.Returned || oldStatus == BookStatus.Status.Damaged)
                {
                    bookModel.Status = newStatus;
                }
                else
                {
                    return UnprocessableEntity($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Returned} or {BookStatus.Status.Damaged}.");
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
                    return UnprocessableEntity($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Available}.");
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
                    return UnprocessableEntity($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Borrowed}.");
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
                    return UnprocessableEntity($"Request could not be processed. Status {newStatus} can only be set if previous status was {BookStatus.Status.Available} or {BookStatus.Status.Returned}.");
                }
            }
            else
            {
                return BadRequest($"Request could not be processed. Possible statuses to be set: {BookStatus.Status.Available}, {BookStatus.Status.Borrowed}, {BookStatus.Status.Damaged}, {BookStatus.Status.Returned}");
            }

            await _context.SaveChangesAsync();

            return Ok(bookModel.ToBookDto());
        }

        [HttpDelete]
        [Route("deleteBook/{bookId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int bookId)
        {
            var bookModel = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (bookModel == null)
            {
                return NotFound();
            }

            _context.Books.Remove(bookModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}