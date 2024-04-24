using Cw3.Models;
using Cw3.Models.DTOs;
using System.Data.SqlClient;

namespace Cw3.Repositories
{
    public interface IAnimalRepository
    {
        public Task AddAnimal(Animal animal);
        public Task<bool> IsInDB(int id);
        public Task EditAnimal(int id, PutAdnimalDTO putAdnimalDTO);
        public Task DeleteAnimal(int id);
        public Task<List<Animal>> GetAnimals(string orderBy);
    }


    public class AnimalRepository : IAnimalRepository
    {
        private readonly IConfiguration _configuration;

        public AnimalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddAnimal(Animal animal)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = new SqlCommand("Insert into Animal (IDAnimal, Name, Description, Category, Area) values (@1, @2, @3, @4, @5)", conn);
                command.Parameters.AddWithValue("@1", animal.IDAnimal);
                command.Parameters.AddWithValue("@2", animal.Name);
                command.Parameters.AddWithValue("@3", animal.Description);
                command.Parameters.AddWithValue("@4", animal.Category);
                command.Parameters.AddWithValue("@5", animal.Area);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();

            }
        }

        public async Task EditAnimal(int id, PutAdnimalDTO putAdnimalDTO)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = new SqlCommand("Update animal set Name=@2, Description=@3, Category=@4, Area=@5 where IDAnimal=@1", conn);
                command.Parameters.AddWithValue("@1", id);
                command.Parameters.AddWithValue("@2", putAdnimalDTO.Name);
                command.Parameters.AddWithValue("@3", putAdnimalDTO.Description);
                command.Parameters.AddWithValue("@4", putAdnimalDTO.Category);
                command.Parameters.AddWithValue("@5", putAdnimalDTO.Area);

                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();

            }
        }

        public async Task<bool> IsInDB(int id)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = new SqlCommand($"Select IDAnimal from Animal where IDAnimal=@1", conn);
                command.Parameters.AddWithValue("@1", id);
                await conn.OpenAsync();
                if (await command.ExecuteScalarAsync() is not null)
                {
                    return true;
                }
                return false;
            }
        }
        public async Task DeleteAnimal(int id)
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = new SqlCommand($"DELETE from Animal where IDAnimal=@1", conn);
                command.Parameters.AddWithValue("@1", id);
                await conn.OpenAsync();
                await command.ExecuteNonQueryAsync();
               
            }
        }

        public async Task<List<Animal>> GetAnimals(string orderBy)
        {
            var animalsList = new List<Animal>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var command = new SqlCommand($"select * from Animal order by {orderBy} ASC", conn);
                await conn.OpenAsync();

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    animalsList.Add(new Animal
                    {
                        IDAnimal = reader.GetInt32(0),
                        Name = reader.GetString(1).Trim(),
                        Description = reader.GetString(2).Trim(),
                        Category = reader.GetString(3).Trim(),
                        Area = reader.GetString(4).Trim()
                    });
                }

            }
            return animalsList;
        }
    }
}
