using Application.DTOs;
using Domain.Entities;

namespace Application.Abstractions.Interfaces
{
    public interface IAccountService
    {
        public Task<Account?> GetByIdAsync(int id);
        public Task<Account?> GetByEmailAsync(string email);
        public Task<IEnumerable<Account>> SearchAsync(AccountFilter filter, int from = 0, int size = 10);
        public Task<int> RegisterAsync(Account account); 
        public Task<int> UpdateAsync(Account entity);
        public Task<int> RemoveAsync(Account entity);  
    }
}
