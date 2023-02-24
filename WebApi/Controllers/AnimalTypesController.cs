using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("animals/types")]
    [ApiController]
    public class AnimalTypesController
        : ControllerBase
    {
        private readonly ILogger<AnimalTypesController> _logger;

        public AnimalTypesController(ILogger<AnimalTypesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{typeId:long}")]
        public async Task<ActionResult<GetAnimalTypeDto>> Get(long typeId)
        {
            throw new NotImplementedException();
        }
    }
}
