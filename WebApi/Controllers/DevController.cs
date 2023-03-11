using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("dev")]
    [ApiController]
    public class DevController
        : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DevController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("database/create")]
        public void CreateDb()
        {
            _applicationDbContext.Database.EnsureCreated();
        }

        [HttpGet("database/delete")]
        public void DeleteDb()
        {
            _applicationDbContext.Database.EnsureDeleted();
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<ActionResult> Auth()
        {

            return Ok();
        }


    }
}
