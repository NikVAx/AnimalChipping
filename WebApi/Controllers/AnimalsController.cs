using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Animal;
using WebApi.DTOs.VisitedLocationPoint;

namespace WebApi.Controllers
{
    [Route("animals")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AnimalsController :
        ControllerBase
    {
        private readonly ILogger<AnimalsController> _logger;
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;

        public AnimalsController(
            ILogger<AnimalsController> logger,
            IAnimalService animalService,
            IMapper mapper)
        {
            _logger = logger;
            _animalService = animalService;
            _mapper = mapper;
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetAnimalDto>>> Search(
            [FromQuery] AnimalFilterDto filterDto,
            int from = 0,
            int size = 10)
        {

            if(ModelState.IsValid == false || from < 0 || size <= 0)
                return BadRequest();

            var filter = _mapper
                .Map<AnimalFilter>(filterDto);

            var animals = await _animalService
                .SearchAsync(filter, from, size);

            var result = animals
                .Select(animal => _mapper
                    .Map<GetAnimalDto>(animal));

            return Ok(result);
        }

        [HttpGet("{animalId:long}")]
        public async Task<ActionResult<GetAnimalDto>> Get(
            long animalId)
        {
            if(animalId <= 0)
                return BadRequest();

            var animal = await _animalService
                .GetByIdAsync(animalId);

            if(animal == null)
                return NotFound();

            var result = _mapper
                .Map<GetAnimalDto>(animal);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<GetAnimalDto>> Create(
            CreateAnimalDto createAnimalDto)
        {
            if(!createAnimalDto.AnimalTypes.Any() ||
                createAnimalDto.AnimalTypes.Where(x => x <= 0).Count() > 0 ||
                createAnimalDto.Weight <= 0 ||
                createAnimalDto.Height <= 0 ||
                createAnimalDto.Length <= 0 ||
                createAnimalDto.ChipperId <= 0 ||
                createAnimalDto.ChippingLocationId <= 0)
                return BadRequest();

            var animal = _mapper
                .Map<Animal>(createAnimalDto);

            await _animalService
                .CreateAsync(animal);

            var createdAnimal = await _animalService
                .GetByIdAsync(animal.Id);

            var result = _mapper
                .Map<GetAnimalDto>(createdAnimal);

            return Created($"animals/{animal.Id}", result);
        }

        [HttpPut("{animalId:long}")]
        public async Task<ActionResult<GetAnimalDto>> Update(
            long animalId,
            UpdateAnimalDto updateAnimalDto)
        {

            if( animalId <= 0 ||
                updateAnimalDto.Weight <= 0 ||
                updateAnimalDto.Height <= 0 ||
                updateAnimalDto.Length <= 0 ||
                updateAnimalDto.ChipperId <= 0 ||
                updateAnimalDto.ChippingLocationId <= 0)
                return BadRequest();

            var animal = _mapper
                .Map<Animal>(updateAnimalDto);
            animal.Id = animalId;

            await _animalService
                .UpdateAsync(animal);

            var updatedAnimal = await _animalService
                .GetByIdAsync(animal.Id);

            var result = _mapper
                .Map<GetAnimalDto>(updatedAnimal);

            return Ok(result);
        }

        [HttpDelete("{animalId:long}")]
        public async Task<ActionResult> Delete(long animalId)
        {
            if (animalId <= 0)
                return BadRequest();

            await _animalService
                .DeleteAsync(animalId);

            return Ok();
        }

        [HttpPost("{animalId:long}/types/{typeId:long}")]
        public async Task<ActionResult> AddAnimalType(long animalId, long typeId)
        {
            throw new NotImplementedException();
        }

    }
}
