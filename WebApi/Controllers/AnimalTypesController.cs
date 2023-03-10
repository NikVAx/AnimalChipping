using Application.Abstractions.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Animal;
using WebApi.DTOs.AnimalType;

namespace WebApi.Controllers
{
    [Route("animals/types")]
    [ApiController]
    public class AnimalTypesController
        : ControllerBase
    {
        private readonly ILogger<AnimalTypesController> _logger;
        private readonly IAnimalTypeService _animalTypeService;
        private readonly IMapper _mapper;

        public AnimalTypesController(
            ILogger<AnimalTypesController> logger,
            IAnimalTypeService animalTypeService,
            IMapper mapper)
        {
            _logger = logger;
            _animalTypeService = animalTypeService;
            _mapper = mapper;
        }

        [HttpGet("{typeId:long}")]
        public async Task<ActionResult<GetAnimalTypeDto>> Get(long typeId)
        {
            if(typeId <= 0)
                return BadRequest();

            var animalType = await _animalTypeService
                .GetByIdAsync(typeId);

            if(animalType == null)
                return NotFound();

            var result = _mapper
                .Map<GetAnimalTypeDto>(animalType);

            return Ok(result);
        }
    }
}
