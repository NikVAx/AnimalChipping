using Application.Abstractions.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs.Account;

namespace WebApi.Controllers
{
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController :
        ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IAccountService accountService,
            IMapper mapper)
        {
            _logger = logger;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("/registration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<GetAccountDto>> Register(
            RegisterUpdateAccountDto registerDto)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous) == false)
                return Forbid();

            var account = _mapper
                .Map<Account>(registerDto);

            await _accountService
                .RegisterAsync(account);

            var result = _mapper
                .Map<GetAccountDto>(account);

            return Created(@$"accounts/{result.Id}", result);
        }
    }
}
