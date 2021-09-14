using NtierDemo.Core.Entities;
using NtierDemo.Entities.Concrete;
using System.Collections.Generic;

namespace NtierDemo.Entities.Dtos
{
    public class BookForGetDto : IDto
    {
        public int Id { get; set; }
        public AuthorForGetDto Author { get; set; }
        public string Title { get; set; }
        public string RelaseYear { get; set; }
        public IEnumerable<BookType> BookTypes { get; set; }
    }
}
