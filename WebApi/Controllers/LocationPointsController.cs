using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("locations")]
    [ApiController]
    public class LocationPointsController
        : ControllerBase
    {
        private readonly ILogger<LocationPointsController> _logger;

        public LocationPointsController(ILogger<LocationPointsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{pointId:long}")]
        public async Task<ActionResult<GetLocationPointDto>> Get(long pointId)
        {
            throw new NotImplementedException();
        }
    }
}
