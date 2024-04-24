using Cw3.Models;
using Cw3.Models.DTOs;
using Cw3.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Cw3.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAnimalRepository _animalRepository;

        public AnimalsController(IConfiguration configuration, IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAnimals(string? orderBy)
        {
            if (orderBy == null)
            {
                orderBy = "name";
            }

            return Ok(await _animalRepository.GetAnimals(orderBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewAnimal(PostAnimalDTO postAnimalDTO)
        {
            if(await _animalRepository.IsInDB(postAnimalDTO.IDAnimal)) 
            {
                return Conflict();
            }

            await _animalRepository.AddAnimal(new Animal
            {
                IDAnimal = postAnimalDTO.IDAnimal,
                Name = postAnimalDTO.Name,
                Description = postAnimalDTO.Description,
                Category = postAnimalDTO.Category,
                Area = postAnimalDTO.Area
            });

            return Created($"api/animals/{postAnimalDTO.IDAnimal}",postAnimalDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAnimal(string id, PutAdnimalDTO putAdnimalDTO)
        {
            if (!(await _animalRepository.IsInDB(int.Parse(id))))
            {
                return NotFound();
            }

            await _animalRepository.EditAnimal(int.Parse(id),putAdnimalDTO);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleAnimal(string id)
        {
            if (!await _animalRepository.IsInDB(int.Parse(id)))
            {
                return NotFound();
            }

            await _animalRepository.DeleteAnimal(int.Parse(id));

            return Ok();

        }
    }
}