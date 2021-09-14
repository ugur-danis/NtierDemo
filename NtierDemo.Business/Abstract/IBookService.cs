using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NtierDemo.Business.Abstract
{
    public interface IBookService
    {
        IList<Book> GetList(Expression<Func<Book, bool>> filter = null);
        Book Get(Expression<Func<Book, bool>> filter);
        Book GetById(int id);
        Book GetByTitle(string title);
        Book GetByAuthorId(int authorId);
        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
