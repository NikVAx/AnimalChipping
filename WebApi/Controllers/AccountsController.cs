using Application.Abstractions.Interfaces;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using WebApi.DTOs.Account;

namespace WebApi.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountsController
        : ControllerBase
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

            var result = new GetAccountDto()
            {
                Id = account.Id,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName
            };

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetAccountDto>>> Search(
            [FromQuery] AccountFilterDto filterDto,
            int from = 0,
            int size = 10)
        {
            if(!ModelState.IsValid || from < 0 || size <= 0)
                return BadRequest();

            var filter = _mapper
                .Map<AccountFilter>(filterDto);

            var result = await _accountService
                .SearchAsync(filter, from, size);

            return Ok(result);
        }
    }
}
