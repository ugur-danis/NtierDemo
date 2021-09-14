using Microsoft.AspNetCore.Mvc;
using NtierDemo.Business.Abstract;
using NtierDemo.Entities.Concrete;
using NtierDemo.Entities.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace NtierDemo.WebApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthorService _authorService;

        public AuthorController(IAuthorService authorService) => _authorService = authorService;

        [HttpGet]
        public IActionResult Get()
        {
            List<AuthorForGetDto> authorForGets = new List<AuthorForGetDto>();
            List<Author> authors = _authorService.GetList().ToList();
            foreach (Author author in authors)
            {
                authorForGets.Add(new AuthorForGetDto
                {
                    Id = author.Id,
                    Name = author.Name,
                    LastName = author.LastName
                });
            }
            return Ok(authorForGets);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Author author = _authorService.GetById(id);
            if (author == null)
                return NotFound();
            return Ok(new AuthorForGetDto
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName
            });
        }

        [HttpPost]
        public IActionResult Add(AuthorForPostDto author)
        {
            _authorService.Add(new Author
            {
                Name = author.Name,
                LastName = author.LastName,
            });
            return Ok();
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, AuthorForPostDto author)
        {
            Author foundAuthor = _authorService.GetById(id);
            if (foundAuthor == null) return NotFound();

            _authorService.Update(new Author
            {
                Id = id,
                Name = author.Name,
                LastName = author.LastName
            });
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Author author = _authorService.GetById(id);
            if (author == null) return NotFound();
            _authorService.Delete(author);
            return Ok();
        }
    }
}