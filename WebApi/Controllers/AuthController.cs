using Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("registration")]
    [ApiController]
    public class AuthController :
        ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IAccountService _accountService;

        public AuthController(
            ILogger<AuthController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public class RegisterAccountDto
        {
        
        }

        //[HttpPost]
        //public Task<int> Register(RegisterAccountDto registerAccountDto)
        //{
        //    _accountService.RegisterAsync();
        //}
    }
}
