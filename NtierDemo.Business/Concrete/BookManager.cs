using NtierDemo.Business.Abstract;
using NtierDemo.DataAccess.Abstract;
using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NtierDemo.Business.Concrete
{
    public class BookManager : IBookService
    {
        private IBookDal _bookDal;
        public BookManager(IBookDal bookDal) => _bookDal = bookDal;

        public IList<Book> GetList(Expression<Func<Book, bool>> filter = null) => _bookDal.GetList(filter);
        public Book Get(Expression<Func<Book, bool>> filter) => _bookDal.Get(filter);
        public Book GetById(int id) => _bookDal.Get(x => x.BookId == id);
        public Book GetByTitle(string title) => _bookDal.Get(x => x.Title == title);
        public Book GetByAuthorId(int authorId) => _bookDal.Get(x => x.Author.Id == authorId);

        public void Add(Book book) => _bookDal.Add(book);
        public void Delete(Book book) => _bookDal.Delete(book);
        public void Update(Book book) => _bookDal.Update(book);
    }
}