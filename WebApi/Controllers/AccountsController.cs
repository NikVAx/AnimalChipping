using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs.Account;

namespace WebApi.Controllers
{
    [Route("accounts")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AccountsController :
        ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(
            ILogger<AccountsController> logger,
            IAccountService accountService,
            IMapper mapper)
        {
            _logger = logger;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("{accountId:int}")]
        public async Task<ActionResult<GetAccountDto>> Get(int accountId)
        {
            if(accountId <= 0)
                return BadRequest();

            var account = await _accountService
                .GetByIdAsync(accountId);

            if(account == null)
                return NotFound();

            var result = _mapper
                .Map<GetAccountDto>(account);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetAccountDto>>> Search(
            [FromQuery] AccountFilterDto filterDto,
            int from = 0,
            int size = 10)
        {
            if(ModelState.IsValid == false || from < 0 || size <= 0)
                return BadRequest();

            var filter = _mapper
                .Map<AccountFilter>(filterDto);

            var accounts = await _accountService
                .SearchAsync(filter, from, size);

            var result = accounts
                .Select(account => _mapper
                    .Map<GetAccountDto>(account));

            return Ok(result);
        }

        [HttpPut("{accountId:int}")]
        public async Task<ActionResult<GetAccountDto>> Update(
            int accountId,
            RegisterUpdateAccountDto updateAccountDto)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            string id = User.FindFirstValue("Id");

            var account = await _accountService
                .GetByIdAsync(accountId);

            if(account == null || Convert.ToInt32(id) != accountId)
                return Forbid();

            account.FirstName = updateAccountDto.FirstName;
            account.LastName  = updateAccountDto.LastName;
            account.Email     = updateAccountDto.Email;
            account.Password  = updateAccountDto.Password;

            await _accountService
                .UpdateAsync(account);

            var result = _mapper
                .Map<GetAccountDto>(account);

            return Ok(result);
        }

        [HttpDelete("{accountId:int}")]
        public async Task<ActionResult> Delete(int accountId)
        {
            if(User.HasClaim(AppClaims.Anonymous, AppClaims.Anonymous))
                return Unauthorized();

            if(accountId <= 0)
                return BadRequest();

            var id = User.FindFirstValue("Id");

            var account = await _accountService
                .GetByIdAsync(accountId);

            if(account == null || Convert.ToInt32(id) != accountId)
                return Forbid();

            await _accountService
                .DeleteAsync(accountId);

            return Ok();
        }
    }
}
