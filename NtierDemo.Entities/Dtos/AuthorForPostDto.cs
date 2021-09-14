using NtierDemo.Core.Entities;

namespace NtierDemo.Entities.Dtos
{
    public class AuthorForPostDto : IDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
