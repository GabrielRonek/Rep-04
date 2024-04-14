using Microsoft.AspNetCore.Mvc;

namespace Cw3
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private readonly string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s24515;Integrated Security=True;Trust Server Certificate=Tru";

        [HttpGet]
        public IActionResult GetAnimals(string orderBy = "name")
        {
            string orderByClause = $"ORDER BY {orderBy}";
            
            List<Animal> animals = new List<Animal>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM Animals {orderByClause}";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Animal animal = new Animal
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Category = reader["Category"].ToString(),
                        Area = reader["Area"].ToString()
                    };
                    animals.Add(animal);
                }
            }

            return Ok(animals);
        }

        [HttpPost]
        public IActionResult AddAnimal(Animal animal)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Animals (Name, Description, Category, Area) " +
                             "VALUES (@Name, @Description, @Category, @Area)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Dodano zwierze pomyslnie");
                }
                else
                {
                    return BadRequest("Nie udalo sie dodac zwierzecia");
                }
            }
        }
    }

    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
    }
}
