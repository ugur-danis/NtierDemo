using NtierDemo.Business.Abstract;
using NtierDemo.DataAccess.Abstract;
using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NtierDemo.Business.Concrete
{
    public class AuthorManager : IAuthorService
    {
        private IAuthorDal _authorDal;
        public AuthorManager(IAuthorDal authorDal) => _authorDal = authorDal;

        public IList<Author> GetList(Expression<Func<Author, bool>> filter = null) => _authorDal.GetList(filter);
        public Author Get(Expression<Func<Author, bool>> filter) => _authorDal.Get(filter);
        public Author GetById(int id) => _authorDal.Get(x => x.Id == id);
        public Author GetByName(string name) => _authorDal.Get(x => x.Name == name);
        public Author GetByFullName(string name, string lastName) => _authorDal.Get(x => x.Name == name && x.LastName == lastName);
        public Author GetByLastName(string lastName) => _authorDal.Get(x => x.LastName == lastName);

        public void Add(Author author) => _authorDal.Add(author);
        public void Delete(Author author) => _authorDal.Delete(author);
        public void Update(Author author) => _authorDal.Update(author);
    }
}
