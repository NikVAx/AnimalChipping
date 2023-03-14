using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.VisitedLocationPoint;

namespace WebApi.Controllers
{
    [Route("animals/{animalId:long}")]
    [ApiController]
    [Authorize]
    public class AnimalVisitedLocationsController :
        ControllerBase
    {
        private readonly ILogger<AnimalVisitedLocationsController> _logger;
        private readonly IAnimalLocationPointService _animalLocationPointService;
        private readonly IMapper _mapper;

        public AnimalVisitedLocationsController(
            ILogger<AnimalVisitedLocationsController> logger,
            IAnimalLocationPointService animalLocationPointService,
            IMapper mapper)
        {
            _logger = logger;
            _animalLocationPointService = animalLocationPointService;
            _mapper = mapper;
        }


        [HttpGet("locations")]
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

            var points = await _animalLocationPointService
                .SearchAsync(animalId, filter, from, size);

            var result = points.Select(animalVisitedLocation => _mapper
                .Map<GetVisitedLocationPointDto>(animalVisitedLocation));

            return Ok(result);
        }

        [HttpPost("locations/{pointId:long}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetVisitedLocationPointDto>> AddLocation(
            long animalId,
            long pointId)
        {
            if(animalId <= 0 || pointId <= 0)
                return BadRequest();

            var location = await _animalLocationPointService
                .AddAsync(animalId, pointId);

            var result = _mapper
                .Map<GetVisitedLocationPointDto>(location);

            return Created("animals/{animalId}/locations", result);
        }

        [HttpPut("locations")]
        public async Task<ActionResult<GetVisitedLocationPointDto>> UpdateLocations(
            long animalId,
            long pointId)
        {
            if(animalId <= 0 || pointId <= 0)
                return BadRequest();

            throw new NotImplementedException();
            return new GetVisitedLocationPointDto();
        }

        [HttpDelete("locations/{visitedPointId:long}")]
        public async Task<ActionResult> DeleteLocation(
            long animalId,
            long visitedPointId)
        {
            if(animalId <= 0 || visitedPointId <= 0)
                return BadRequest();

            await _animalLocationPointService
                .RemoveAsync(animalId, visitedPointId);

            return Ok();
        }
    }
}
