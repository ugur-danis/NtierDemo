using NtierDemo.Core.Entities;

namespace NtierDemo.Entities.Dtos
{
    public class AuthorForGetDto : AuthorForPostDto, IDto
    {
        public int Id { get; set; }
    }
}
