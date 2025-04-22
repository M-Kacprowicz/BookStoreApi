using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Data;
using BookStoreApi.Dtos.Book;
using BookStoreApi.Mappers;
using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var books = _context.Books.ToList().Select(s => s.ToBookDto());

            return Ok(books);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateBookRequestDto bookDto)
        {
            var bookModel = bookDto.ToBookFromCreateDto();
            _context.Books.Add(bookModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = bookModel.Id }, bookModel);
        }

        [HttpPut]
        [Route("updatebook/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateBookRequestDto updateDto)
        {
            var bookModel = _context.Books.FirstOrDefault(x => x.Id == id);

            if (bookModel == null)
            {
                return NotFound();
            }

            bookModel.Author = updateDto.Author;
            bookModel.Title = updateDto.Title;
            bookModel.Isbn = updateDto.Isbn;

            _context.SaveChanges();

            return Ok(bookModel);
        }

        [HttpPut]
        [Route("updatestatus/{id}")]
        public IActionResult UpdateStatus([FromRoute] int id, [FromBody] UpdateBookStatusRequestDto updateStatusDto)
        {
            var bookModel = _context.Books.FirstOrDefault(x => x.Id == id);

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

            _context.SaveChanges();

            return Ok(bookModel);
        }
    }
}