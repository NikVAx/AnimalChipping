using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attibutes.ValidationAttibutes;
using WebApi.DTOs.VisitedLocationPoint;

namespace WebApi.Controllers
{
    [Route("animals/{animalId:long}/locations")]
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


        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<GetVisitedLocationPointDto>>> GetLocations(
            [MinInt64(1)] long animalId,
            [FromQuery] LocationFilterDto filterDto,
            [MinInt32(0)] int from = 0,
            [MinInt32(1)] int size = 10)
        {
            var filter = _mapper
                .Map<LocationFilter>(filterDto);

            var points = await _animalLocationPointService
                .SearchAsync(animalId, filter, from, size);

            var result = points.Select(animalVisitedLocation => _mapper
                .Map<GetVisitedLocationPointDto>(animalVisitedLocation));

            return Ok(result);
        }

        [HttpPost("{pointId:long}")]
        [Authorize(policy: ApplicationPolicies.Identified)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetVisitedLocationPointDto>> AddLocation(
            [MinInt64(1)] long animalId,
            [MinInt64(1)] long pointId)
        {
            var location = await _animalLocationPointService
                .AddAsync(animalId, pointId);

            var result = _mapper
                .Map<GetVisitedLocationPointDto>(location);

            return Created("animals/{animalId}/locations", result);
        }

        [Authorize(policy: ApplicationPolicies.Identified)]
        [HttpPut("")]
        public async Task<ActionResult<GetVisitedLocationPointDto>> UpdateLocations(
            [MinInt64(1)] long animalId,
            UpdateVisitedLocationPointDto updateDto)
        {
            var location = await _animalLocationPointService
                .UpdateAsync(animalId, updateDto.VisitedLocationPointId, updateDto.LocationPointId);

            var result = _mapper
                .Map<GetVisitedLocationPointDto>(location);

            return Ok(result);
        }

        [Authorize(policy: ApplicationPolicies.Identified)]
        [HttpDelete("{visitedPointId:long}")]
        public async Task<ActionResult> DeleteLocation(
            [MinInt64(1)] long animalId,
            [MinInt64(1)] long visitedPointId)
        {

            await _animalLocationPointService
                .RemoveAsync(animalId, visitedPointId);

            return Ok();
        }
    }
}
