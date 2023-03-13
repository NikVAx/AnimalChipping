using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text;
using System.Security.Claims;
using Application.Abstractions.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace WebApi.Handlers
{
    public class BasicAuthenticationHandler :
        AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAccountService _accountService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAccountService accountService) :
            base(options, logger, encoder, clock)
        {
            _accountService = accountService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var errorMessage = "Invalid email or password";

            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return AnonymousAuthenticateResult();
            }

            var auth = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(auth.Parameter);
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");

            var email = credentials[0];
            var password = credentials[1];

            var account = await _accountService
                .GetByEmailAsync(email);

            if(account == null)
            {
                return AuthenticateResult.Fail(errorMessage);
            }

            if(_accountService.VerifyPassword(account, password) == PasswordVerificationResult.Success)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Id", account.Id.ToString())
                };

                var ticket = CreateTicket(claims);
                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail(errorMessage);
        }

        private AuthenticationTicket CreateTicket(Claim[] claims)
        {
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);        
        }

        private AuthenticateResult AnonymousAuthenticateResult()
        {

            var claims = new[]
            {
                    new Claim(ClaimTypes.Anonymous, "Anonymous")
            };

            return AuthenticateResult.Success( CreateTicket(claims) );
        }


    }
}
