using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Interfaces
{
    public interface IAccountService
    {
        public Task<Account?> GetByIdAsync(int id);
        public Task<Account?> GetByEmailAsync(string email);
        public Task<IEnumerable<Account>> SearchAsync(AccountFilter filter, int from = 0, int size = 10); 
        public Task RegisterAsync(Account account); 
        public PasswordVerificationResult VerifyPassword(Account account, string password);
        public Task UpdateAsync(Account entity);
        public Task DeleteAsync(int id);
    }
}
