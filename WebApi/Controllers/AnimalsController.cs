using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
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


        [HttpGet("{animalId:long}")]
        public async Task<ActionResult<GetAnimalDto>> Get(long animalId)
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

        
        [HttpGet("{animalId:long}/locations")]
        public async Task<ActionResult<IEnumerable<GetVisitedLocationPointDto>>> GetLocations(
            long animalId,
            [FromQuery] LocationFilterDto filterDto,
            int from = 0,
            int size = 10)
        {
            if(ModelState.IsValid == false || from < 0 || size <= 0)
                return BadRequest();

            var filter = _mapper
                .Map<LocationFilter>(filterDto);

            var result = await _animalService
                .SearchAnimalVisitedLocationsAsync(animalId, filter, from, size);

            return Ok(result);
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
    }
}
