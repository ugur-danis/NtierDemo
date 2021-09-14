using MySqlConnector;
using NtierDemo.Core.Helpers.DataAccess.AdoNet;
using NtierDemo.DataAccess.Abstract;
using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace NtierDemo.DataAccess.Concrete.AdoNet
{
    public class AdoAuthorDal : IAuthorDal
    {
        private readonly string _connectionString;
        private readonly AdoSqlQueryHelper _queryHelper;

        public AdoAuthorDal(string connectionString)
        {
            _connectionString = connectionString;
            _queryHelper = new AdoSqlQueryHelper();
        }

        public IList<Author> GetList(Expression<Func<Author, bool>> filter = null)
        {
            IList<Author> authors = new List<Author>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"{_queryHelper.Select("authors")}" +
                        $"{_queryHelper.OrderBy(AdoSqlQueryHelper.OrderByMethods.DESC, new string[] { "id" })}"
                        ;

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            authors.Add(new Author
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                LastName = reader.GetString(2)
                            });
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            if (filter == null)
                return authors;
            else return authors.AsQueryable().Where(filter).ToList();
        }

        public Author Get(Expression<Func<Author, bool>> filter) => GetList(filter).FirstOrDefault();

        public void Add(Author author)
        {
            Author foundAuthor = Get(x => x.Name == author.Name && x.LastName == author.LastName);

            if (foundAuthor != null) return;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    string[] columnNames = new string[] { "name", "last_name" };
                    string[] values = new string[] { author.Name, author.LastName };

                    command.CommandText = $"{_queryHelper.Insert("authors", columnNames, values)}; SELECT COUNT(*) FROM authors";
                    connection.Open();
                    author.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                connection.Close();
            }
        }

        public void Delete(Author entity)
        {
            Author author = Get(x => x.Id == entity.Id);
            if (author == null) return;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = $"{_queryHelper.Delete("authors_books", _queryHelper.Where(author.Id.ToString(), "author_id"))}";
                    command.ExecuteNonQuery();

                    command.CommandText = $"{_queryHelper.Delete("authors", _queryHelper.Where(author.Id.ToString(), "id"))}";
                    command.ExecuteNonQuery();

                    if (author.Books != null)
                        foreach (var book in author.Books)
                        {
                            command.CommandText = $"{_queryHelper.Delete("books_types", _queryHelper.Where(book.BookId.ToString(), "book_id"))}";
                            command.ExecuteNonQuery();

                            command.CommandText = $"{_queryHelper.Delete("books", _queryHelper.Where(book.BookId.ToString(), "id"))}";
                            command.ExecuteNonQuery();
                        }
                }
                connection.Close();
            }
        }

        public void Update(Author author)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    IDictionary<string, object> authorsColumnNameValues = new Dictionary<string, object>();
                    authorsColumnNameValues.Add("name", author.Name);
                    authorsColumnNameValues.Add("last_name", author.LastName);

                    connection.Open();

                    command.CommandText = $"{_queryHelper.Update("authors", authorsColumnNameValues, _queryHelper.Where("id", author.Id.ToString()))}";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
