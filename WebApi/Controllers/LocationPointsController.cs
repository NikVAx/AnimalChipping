using Application.Abstractions.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.LocationPoint;

namespace WebApi.Controllers
{
    [Route("locations")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class LocationPointsController :
        ControllerBase
    {
        private readonly ILogger<LocationPointsController> _logger;
        private readonly ILocationPointService _locationPointService;
        private readonly IMapper _mapper;

        public LocationPointsController(
            ILogger<LocationPointsController> logger,
            ILocationPointService locationPointService,
            IMapper mapper)
        {
            _logger = logger;
            _locationPointService = locationPointService;
            _mapper = mapper;
        }

        [HttpGet("{pointId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetLocationPointDto>> Get(
            long pointId)
        {
            if(pointId <= 0)
                return BadRequest();

            var point = await _locationPointService
                .GetByIdAsync(pointId);

            if(point == null)
                return NotFound();

            var result = _mapper
                .Map<GetLocationPointDto>(point);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetLocationPointDto>> Create(
            CreateUpdateLocationPointDto createPointDto)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if (ModelState.IsValid == false) 
                return BadRequest();

            var point = _mapper
                .Map<LocationPoint>(createPointDto);

            await _locationPointService
                .CreateAsync(point);

            var result = _mapper
                .Map<GetLocationPointDto>(point);
            
            return Created($@"locations/{point.Id}", result);
        }

        [HttpPut("{pointId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetLocationPointDto>> Update(
            long pointId,
            CreateUpdateLocationPointDto updatePointDto)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(ModelState.IsValid == false || pointId <= 0)
                return BadRequest();

            var point = _mapper
                .Map<LocationPoint>(updatePointDto);
            
            point.Id = pointId;

            await _locationPointService
                .UpdateAsync(point);

            var result = _mapper
                .Map<GetLocationPointDto>(point);

            return Ok(result);
        }

        [HttpDelete("{pointId:long}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(
            long pointId)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(pointId <= 0)
                return BadRequest();

            await _locationPointService
                .DeleteAsync(pointId);

            return Ok();
        }
    }
}
