using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Data;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Interfaces;
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
        private readonly IBookRepository _bookRepo;
        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var books = await _bookRepo.GetAllAsync();

            var booksDto = books.Select(s => s.ToBookDto());

            return Ok(booksDto);
        }

        [ActionName(nameof(GetByIdAsync))]
        [HttpGet]
        [Route("{bookId}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int bookId)
        {
            var book = await _bookRepo.GetByIdAsync(bookId);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpGet]
        [Route("getByAuthor/{author}")]
        public async Task<IActionResult> GetByAuthorAsync([FromRoute] string author)
        {
            try
            {
                var books = await _bookRepo.GetByAuthorAsync(author);
                var booksDto = books.Select(s => s.ToBookDto());
                return Ok(booksDto);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBookRequestDto bookDto)
        {
            var bookModel = bookDto.ToBookFromCreateDto();
            await _bookRepo.CreateAsync(bookModel);
            return CreatedAtAction(nameof(GetByIdAsync), new { bookId = bookModel.Id }, bookModel);
        }

        [HttpPut]
        [Route("updateBookInfo/{bookId}")]
        public async Task<IActionResult> UpdateBookInfoAsync([FromRoute] int bookId, [FromBody] UpdateBookRequestDto updateDto)
        {
            var bookModel = await _bookRepo.UpdateBookInfoAsync(bookId, updateDto);

            if (bookModel == null)
            {
                return NotFound();
            }

            return Ok(bookModel);
        }

        [HttpPut]
        [Route("updateBookStatus/{bookId}")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] int bookId, [FromBody] UpdateBookStatusRequestDto updateStatusDto)
        {
            try
            {
                var bookModel = await _bookRepo.UpdateBookStatusAsync(bookId, updateStatusDto);

                if (bookModel == null)
                {
                    return NotFound();
                }

                return Ok(bookModel.ToBookDto());
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete]
        [Route("deleteBook/{bookId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int bookId)
        {
            var bookModel = await _bookRepo.DeleteAsync(bookId);

            if (bookModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}