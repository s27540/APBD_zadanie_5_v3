using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.AnimalAPIStructure;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController (IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnimals (string orderBy)
    {
        var result = _animalService.GetAnimals(orderBy);

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> AddAnimal (Animal animal)
    {
        var result = _animalService.AddAnimal(animal);

        if (result == 1)
        {
            return Ok( "Animal was added to data base properly" );
        }

        return NotFound( $"Adding animal with id {animal.IdAnimal} to data base failed" );
    }

    [HttpPut("{idAnimal}")]
    public async Task<IActionResult> UpdateAnimal (Animal animal, int idAnimal)
    {
        var result = _animalService.UpdateAnimal(animal, idAnimal);
        if (result == 1)
        {
            return Ok("Animal wad updated in data base");
        }

        return StatusCode((int) HttpStatusCode.InternalServerError, $"Updating animal with id {animal.IdAnimal} in data base failed");
    }

    [HttpDelete("{idAnimal}")]
    public async Task<IActionResult> DeleteAnimal (int idAnimal)
    {
        var result = _animalService.DeleteAnimal(idAnimal);

        if (result == 1)
        {
            return Ok($"Animal with id {idAnimal} was deleted from data base");
        }

        return StatusCode((int) HttpStatusCode.InternalServerError, $"Deleting animal with id {idAnimal} from database failed");
    }
}