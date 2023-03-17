using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attibutes.ValidationAttibutes;
using WebApi.DTOs.Animal;

namespace WebApi.Controllers
{
    [Route("animals")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AnimalsController : ControllerBase
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
            [MinInt32(0)] int from = 0,
            [MinInt32(1)] int size = 10)
        {

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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
            [MinInt64(1)] long animalId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(!ModelState.IsValid ||
               !createAnimalDto.AnimalTypes.Any() ||
                createAnimalDto.AnimalTypes.Any(x => x <= 0))
                return BadRequest(ModelState);

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
            [MinInt64(1)] long animalId,
            UpdateAnimalDto updateAnimalDto)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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
        public async Task<ActionResult> Delete(
            [MinInt64(1)] long animalId)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _animalService
                .DeleteAsync(animalId);

            return Ok();
        }

        [HttpPost("{animalId:long}/types/{typeId:long}")]
        public async Task<ActionResult<GetAnimalDto>> AddAnimalType(
            [MinInt64(1)] long animalId,
            [MinInt64(1)] long typeId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            await _animalService
                .AddAnimalType(animalId, typeId);

            var animal = await _animalService
                .GetByIdAsync(animalId);

            var result = _mapper
                .Map<GetAnimalDto>(animal);

            return Created($@"animals/{animalId}", result);
        }

        [HttpDelete("{animalId:long}/types/{typeId:long}")]
        public async Task<ActionResult<GetAnimalDto>> RemoveAnimalType(
            [MinInt64(1)] long animalId,
            [MinInt64(1)] long typeId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _animalService
                .RemoveAnimalType(animalId, typeId);

            var animal = await _animalService
                .GetByIdAsync(animalId);

            var result = _mapper
                .Map<GetAnimalDto>(animal);

            return Ok(result);
        }

        [HttpPut("{animalId:long}/types")]
        public async Task<ActionResult<GetAnimalDto>> UpdateAnimalType(
            [MinInt64(1)] long animalId,
            EditAnimalTypeDto editDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _animalService
                .UpdateAnimalType(animalId, editDto.OldTypeId, editDto.NewTypeId);

            var animal = await _animalService
                .GetByIdAsync(animalId);

            var result = _mapper
                .Map<GetAnimalDto>(animal);

            return Ok(result);
        }
    }
}
