using NtierDemo.Core.Entities;

namespace NtierDemo.Entities.Concrete
{
    public class BookType : IEntity
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
