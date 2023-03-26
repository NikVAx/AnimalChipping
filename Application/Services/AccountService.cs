using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Account?> GetByEmailAsync(string email)
        {
            var account = await _applicationDbContext.Account
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
            
            return account;
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            var account = await _applicationDbContext.Account
                .AsNoTracking()
                .FirstAsync(id);
            
            return account;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var haveRelationsWithAnimals = _applicationDbContext.Animals
                    .Any(x => x.ChipperId == id);

                if(haveRelationsWithAnimals)
                {
                    throw new OperationException("Account is related with Animal");
                }

                var entity = new Account() { Id = id };

                _applicationDbContext.Account
                    .Remove(entity);

                await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new ForbiddenException("Forbidden", ex);
            }
        }

        public async Task<IEnumerable<Account>> SearchAsync(AccountFilter filter, int from = 0, int size = 10)
        {
            var query = _applicationDbContext.Account.AsQueryable();
            
            if(filter.FirstName != null)
                query = query.Where(x => x.FirstName.ToLower()
                    .Contains(filter.FirstName.ToLower()));         
            
            if(filter.LastName != null)
                query = query.Where(x => x.LastName.ToLower()
                    .Contains(filter.LastName.ToLower()));          
            
            if(filter.Email != null)
                query = query.Where(x => x.Email.ToLower()
                    .Contains(filter.Email.ToLower()));
            
            return await query
                .OrderBy(x => x.Id)
                .Skip(from)
                .Take(size)
                .ToListAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            try
            {
                account.Password = _passwordHasher
                    .HashPassword(account, account.Password);
                
                _applicationDbContext.Account
                    .Update(account);
                
                await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new ForbiddenException("Forbidden", ex);
            }
        }

        public async Task RegisterAsync(Account account)
        {
            try
            {
                account.Password = _passwordHasher
                    .HashPassword(account, account.Password);
               
                _applicationDbContext.Account
                    .Add(account);
                
                await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"Account with email {account.Email} is already exist", ex);
            }
        }

        public PasswordVerificationResult VerifyPassword(Account account, string password)
        {
            return _passwordHasher.VerifyHashedPassword(account, account.Password, password);
        }
    }
}
