using NtierDemo.Core.Entities;
using NtierDemo.Entities.Abstract;
using NtierDemo.Entities.Concrete;
using System.Collections.Generic;

namespace NtierDemo.Entities.Dtos
{
    public class BookForPostDto : IDto
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string RelaseYear { get; set; }
        public IEnumerable<int> BookTypes { get; set; }
    }
}
