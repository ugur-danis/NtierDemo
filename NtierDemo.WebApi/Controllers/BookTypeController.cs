using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace NtierDemo.WebApi.Controllers
{
    [Route("api/types")]
    [ApiController]
    public class BookTypeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            System.Collections.Generic.List<Entities.Concrete.BookType> bookTypes = new System.Collections.Generic.List<Entities.Concrete.BookType>();
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=12345678;persistsecurityinfo=True;database=ntierdemo"))
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM types";
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookTypes.Add(new Entities.Concrete.BookType
                            {
                                TypeId = reader.GetInt32(0),
                                TypeName = reader.GetString(1)
                            });
                        }
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            return Ok(bookTypes);
        }
    }
}
