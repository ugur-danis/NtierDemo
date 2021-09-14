using NtierDemo.Core.Entities;
using System.Collections.Generic;

namespace NtierDemo.Entities.Concrete
{
    public class Author : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
