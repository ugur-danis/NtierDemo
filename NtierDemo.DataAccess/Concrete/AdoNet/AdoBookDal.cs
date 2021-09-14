using MySqlConnector;
using NtierDemo.Core.Helpers.DataAccess.AdoNet;
using NtierDemo.DataAccess.Abstract;
using NtierDemo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NtierDemo.DataAccess.Concrete.AdoNet
{
    public class AdoBookDal : IBookDal
    {
        private readonly string _connectionString;
        private readonly AdoSqlQueryHelper _queryHelper;

        public AdoBookDal(string connectionString)
        {
            _connectionString = connectionString;
            _queryHelper = new AdoSqlQueryHelper();
        }

        public IList<Book> GetList(Expression<Func<Book, bool>> filter = null)
        {
            IList<Book> books = new List<Book>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        string[] columnNames = new string[] {
                        "a_b.book_id", "a_b.author_id", "title", "relase_year", "type_name", "b_t.type_id", "name", "last_name"
                    };
                        command.CommandText =
                        $"{_queryHelper.Select("authors_books", columnNames)}" +
                        $"{_queryHelper.As("a_b")}" +
                        $"{_queryHelper.Join("authors")}" +
                        $"{_queryHelper.As("a")}" +
                        $"{_queryHelper.On("a_b.author_id", "a.id")}" +
                        $"{_queryHelper.Join("books")}" +
                        $"{_queryHelper.As("b")}" +
                        $"{_queryHelper.On("a_b.book_id", "b.id")}" +
                        $"{_queryHelper.Join("books_types")}" +
                        $"{_queryHelper.As("b_t")}" +
                        $"{_queryHelper.On("a_b.book_id", "b_t.book_id")}" +
                        $"{_queryHelper.Join("types")}" +
                        $"{_queryHelper.As("t")}" +
                        $"{_queryHelper.On("b_t.type_id", "t.id")}" +
                        $"{_queryHelper.OrderBy(AdoSqlQueryHelper.OrderByMethods.DESC, new string[] { "b.id" })}"
                        ;

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Book book = books.FirstOrDefault(x => x.BookId == reader.GetInt32(0));
                                if (book == null)
                                {
                                    books.Add(new Book
                                    {
                                        BookId = reader.GetInt32(0),
                                        Author = new Entities.Dtos.AuthorForGetDto
                                        {
                                            Id = reader.GetInt32(1),
                                            Name = reader.GetString(6),
                                            LastName = reader.GetString(7)
                                        },
                                        Title = reader.GetString(2),
                                        RelaseYear = reader.GetString(3),
                                        Types = new List<BookType>
                                    {
                                        new BookType
                                        {
                                            TypeId = reader.GetInt32(5),
                                            TypeName = reader.GetString(4)
                                        }
                                    }
                                    });
                                }
                                else
                                {
                                    List<BookType> bookTypes = book.Types.ToList();
                                    bookTypes.Add(new BookType
                                    {
                                        TypeId = reader.GetInt32(5),
                                        TypeName = reader.GetString(4)
                                    });
                                    book.Types = bookTypes;
                                }
                            }
                            reader.Close();
                            connection.Close();
                        }
                    }
                }
                if (filter == null)
                    return books;
                else return books.AsQueryable().Where(filter).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Book Get(Expression<Func<Book, bool>> filter) => GetList(filter).FirstOrDefault();

        public void Add(Book book)
        {
            Book foundBook = Get(x => x.Author.Id == book.Author.Id && x.Title == book.Title);

            if (foundBook != null) return;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        string[] booksColumnNames = new string[] { "title", "relase_year" };
                        string[] values = new string[] { book.Title, book.RelaseYear };

                        try
                        {
                            connection.Open();
                            command.CommandText = $"{_queryHelper.Insert("books", booksColumnNames, values)}; SELECT COUNT(*) FROM books";
                            book.BookId = Convert.ToInt32(command.ExecuteScalar());

                            command.CommandText = $"{_queryHelper.Insert("authors_books", $"{book.Author.Id}, {book.BookId}")}";
                            command.ExecuteNonQuery();

                            foreach (BookType bookType in book.Types)
                            {
                                command.CommandText = $"{_queryHelper.Insert("books_types", $"{book.BookId}, {bookType.TypeId}")}";
                                command.ExecuteNonQuery();
                            }
                        }

                        catch (Exception)
                        {
                            connection.Close();
                            return;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void Delete(Book book)
        {
            Book foundBook = Get(x => x.BookId == book.BookId);
            if (foundBook == null) return;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = $"{_queryHelper.Delete("books_types", _queryHelper.Where(book.BookId.ToString(), "book_id"))}";
                        command.ExecuteNonQuery();

                        command.CommandText = $"{_queryHelper.Delete("authors_books", _queryHelper.Where(book.BookId.ToString(), "book_id"))}";
                        command.ExecuteNonQuery();

                        command.CommandText = $"{_queryHelper.Delete("books", _queryHelper.Where(book.BookId.ToString(), "id"))}";
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void Update(Book book)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        IDictionary<string, object> booksColumnNameValues = new Dictionary<string, object>();
                        booksColumnNameValues.Add("title", book.Title);
                        booksColumnNameValues.Add("relase_year", book.RelaseYear);

                        connection.Open();

                        command.CommandText = $"{_queryHelper.Update("books", booksColumnNameValues, _queryHelper.Where("id", book.BookId.ToString()))}";
                        command.ExecuteNonQuery();

                        IDictionary<string, object> authorsBooksColumnNameValues = new Dictionary<string, object>();
                        authorsBooksColumnNameValues.Add("author_id", book.Author.Id);

                        command.CommandText = $"{_queryHelper.Update("authors_books", authorsBooksColumnNameValues, _queryHelper.Where("book_id", book.BookId.ToString()))}";
                        command.ExecuteNonQuery();

                        command.CommandText =
                                $"{_queryHelper.Delete("books_types", _queryHelper.Where("book_id", book.BookId.ToString()))}";
                        command.ExecuteNonQuery();

                        foreach (BookType bookType in book.Types)
                        {
                            string[] booksTypesColumnValues = new string[] { book.BookId.ToString(), bookType.TypeId.ToString() };

                            command.CommandText = $"{_queryHelper.Insert("books_types", booksTypesColumnValues)}";
                            command.ExecuteNonQuery();
                        }
                    }
                    connection.Close();
                }
            }

            catch (Exception)
            {
                return;
            }
        }
    }
}
