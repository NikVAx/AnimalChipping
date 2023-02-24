using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("animals")]
    [ApiController]
    public class AnimalsController
        : ControllerBase
    {
        private readonly ILogger<AnimalsController> _logger;

        public AnimalsController(ILogger<AnimalsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{animalId:long}")]
        public async Task<ActionResult<GetAnimalDto>> Get(long animalId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{animalId:long}/locations")]
        public async Task<ActionResult<IEnumerable<GetVisitedLocationPointDto>>> GetLocations(long animalId,
            DateTime? startDateTime, DateTime? endDateTime, int from = 0, int size = 10)
        {
            throw new NotImplementedException();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetAnimalDto>>> Search(DateTime? startDateTime, DateTime? endDateTime,
            int? chipperId, long? chippingLocationIdint, string? lifeStatus, string? gender, int from = 0, int size = 10)
        {
            throw new NotImplementedException();
        }
    }
}
