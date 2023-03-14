using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.VisitedLocationPoint;

namespace WebApi.Controllers
{
    [Route("animals/{animalId:long}")]
    [ApiController]
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

            var result = await _animalLocationPointService
                .SearchAsync(animalId, filter, from, size);
            // TODO: add mapping to result type
            return Ok(result);
        }

        [HttpPost("locations/{pointId:long}")]
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

            return new GetVisitedLocationPointDto();
        }

        [HttpDelete("locations/{pointId:long}")]
        public async Task<ActionResult<GetVisitedLocationPointDto>> DeleteLocation(
            long animalId,
            long visitedPointId)
        {
            if(animalId <= 0 || visitedPointId <= 0)
                return BadRequest();

            return new GetVisitedLocationPointDto();
        }



    }
}
