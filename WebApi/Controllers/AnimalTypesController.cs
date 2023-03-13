using Application.Abstractions.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.AnimalType;

namespace WebApi.Controllers
{
    [Route("animals/types")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AnimalTypesController :
        ControllerBase
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAnimalTypeDto>> Get(
            long typeId)
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetAnimalTypeDto>> Create(
            CreateUpdateAnimalTypeDto createTypeDto)
        {
            if(ModelState.IsValid == false)
                return BadRequest();

            var animalType = _mapper
                .Map<AnimalType>(createTypeDto);

            await _animalTypeService
                .CreateAsync(animalType);

            var result = _mapper
                .Map<GetAnimalTypeDto>(animalType);

            return Created($@"animals/types/{result.Id}", result);
        }

        [HttpPut("{typeId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetAnimalTypeDto>> Update(
            long typeId,
            CreateUpdateAnimalTypeDto updateTypeDto)
        {
            if(ModelState.IsValid == false || typeId <= 0)
                return BadRequest();

            var type = _mapper
                .Map<AnimalType>(updateTypeDto);

            type.Id = typeId;

            await _animalTypeService
                .UpdateAsync(type);

            var result = _mapper
                .Map<GetAnimalTypeDto>(type);

            return Ok(result);
        }
       
        [HttpDelete("{typeId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(
            long typeId)
        {
            if(typeId <= 0)
                return BadRequest();

            await _animalTypeService
                .DeleteAsync(typeId);

            return Ok();
        }

    }
}
