using Microsoft.AspNetCore.Mvc;
using NtierDemo.Business.Abstract;
using NtierDemo.Entities.Concrete;
using NtierDemo.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NtierDemo.WebApi.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _bookService;

        public BookController(IBookService bookService) => _bookService = bookService;

        [HttpGet]
        public IActionResult Get()
        {
            List<BookForGetDto> bookForGets = new List<BookForGetDto>();
            List<Book> books = _bookService.GetList().ToList();
            foreach (Book book in books)
            {
                bookForGets.Add(new BookForGetDto
                {
                    Id = book.BookId,
                    Author = book.Author,
                    Title = book.Title,
                    RelaseYear = book.RelaseYear,
                    BookTypes = book.Types
                });
            }
            return Ok(bookForGets);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Book book = _bookService.GetById(id);
            if (book == null)
                return NotFound();
            return Ok(new BookForGetDto
            {
                Id = book.BookId,
                Author = book.Author,
                Title = book.Title,
                RelaseYear = book.RelaseYear,
                BookTypes = book.Types
            });
        }

        [HttpPost]
        public IActionResult Add(BookForPostDto book)
        {
            List<BookType> bookTypes = new List<BookType>();
            foreach (int typeId in book.BookTypes)
                bookTypes.Add(new BookType { TypeId = typeId });

            _bookService.Add(new Book
            {
                Author = new AuthorForGetDto { Id = book.AuthorId },
                Title = book.Title,
                RelaseYear = book.RelaseYear,
                Types = bookTypes
            });
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, BookForPostDto book)
        {
            Book foundBook = _bookService.GetById(id);
            if (foundBook == null) return NotFound();

            List<BookType> bookTypes = new List<BookType>();
            foreach (int typeId in book.BookTypes)
                bookTypes.Add(new BookType { TypeId = typeId });

            _bookService.Update(new Book
            {
                BookId = id,
                Author = new AuthorForGetDto { Id = book.AuthorId },
                Title = book.Title,
                RelaseYear = book.RelaseYear,
                Types = bookTypes
            });
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Book book = _bookService.GetById(id);
            if (book == null) return NotFound();
            _bookService.Delete(book);
            return Ok();
        }
    }
}