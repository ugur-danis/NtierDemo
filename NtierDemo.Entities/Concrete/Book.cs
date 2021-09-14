using NtierDemo.Core.Entities;
using System.Collections.Generic;

namespace NtierDemo.Entities.Concrete
{
    public class Book : IEntity
    {
        public int BookId { get; set; }
        public Dtos.AuthorForGetDto Author { get; set; }
        public string Title { get; set; }
        public IEnumerable<BookType> Types { get; set; }
        public string RelaseYear { get; set; }
    }
}
