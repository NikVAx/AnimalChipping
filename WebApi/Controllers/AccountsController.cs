using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController
        : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }


        [HttpGet("{accountId:int}")]
        public async Task<ActionResult<GetAccountDto>> Get(int accountId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetAccountDto>>> Search(string? firstName, string? lastName,
            string? email, int from = 0, int size = 10)
        {
            throw new NotImplementedException();
        }


    }
}
