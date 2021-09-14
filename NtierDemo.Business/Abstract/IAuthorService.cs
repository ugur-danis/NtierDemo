using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NtierDemo.Business.Abstract
{
    public interface IAuthorService
    {
        IList<Author> GetList(Expression<Func<Author, bool>> filter = null);
        Author Get(Expression<Func<Author, bool>> filter);
        Author GetById(int id);
        Author GetByName(string name);
        Author GetByLastName(string lastName);
        Author GetByFullName(string name, string lastName);
        void Add(Author author);
        void Update(Author author);
        void Delete(Author author);
    }
}
