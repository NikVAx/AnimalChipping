using Application.Abstractions.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.LocationPoint;

namespace WebApi.Controllers
{
    [Route("locations")]
    [Produces("application/json")]
    [ApiController]
    public class LocationPointsController
        : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLocationPointDto>> Get(long pointId)
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
        public async Task<ActionResult<GetLocationPointDto>> Post(CreateLocationPointDto createPointDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var point = _mapper
                .Map<LocationPoint>(createPointDto);

            await _locationPointService
                .AddAsync(point);

            var result = _mapper
                .Map<GetLocationPointDto>(point);
            
            return Created(HttpContext.Request.PathBase + $@"/{point.Id}", result);
        }
    }
}
