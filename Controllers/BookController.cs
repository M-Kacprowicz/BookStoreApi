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
        [Route("{id}")]
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
            bookModel.Status = updateDto.Status;

            _context.SaveChanges();

            return Ok(bookModel);
        }
    }
}