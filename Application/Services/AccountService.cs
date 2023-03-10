using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.Services
{
    public class AccountService :
        IAccountService
    {

        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountService(
            IApplicationDbContext applicationDbContext, 
            IPasswordHasher<Account> passwordHasher)
        {
            _applicationDbContext = applicationDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Account.FindAsync(id);
        }

        // TODO: Separate account and account.password to different objects (e.g. like AccountData and Credentials) for registration action 

        public async Task<int> RegisterAsync(Account account)
        {
            var password = _passwordHasher.HashPassword(account, account.Password);
            account.Password = password;
            _applicationDbContext.Account.Add(account);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(Account entity)
        {
            _applicationDbContext.Account.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Account>> SearchAsync(AccountFilter filter, int from = 0, int size = 10)
        {
            var query = _applicationDbContext.Account.AsQueryable();

            if(filter.FirstName != null)
                query.Where(x => x.FirstName.Contains(filter.FirstName, StringComparison.OrdinalIgnoreCase));
            if(filter.LastName != null)
                query.Where(x => x.LastName.Contains(filter.LastName, StringComparison.OrdinalIgnoreCase));
            if(filter.Email != null)
                query.Where(x => x.Email.Contains(filter.Email, StringComparison.OrdinalIgnoreCase));

            return await query
                .OrderBy(x => x.Id)
                .Skip(from)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(Account entity)
        {
            _applicationDbContext.Account.Update(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }
    }
}
