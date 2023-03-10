using Application.Abstractions.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AnimalTypeService :
        IAnimalTypeService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AnimalTypeService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> AddAsync(AnimalType entity)
        {
            _applicationDbContext.AnimalTypes.Add(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(AnimalType entity)
        {
            _applicationDbContext.AnimalTypes.Update(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(AnimalType entity)
        {
            _applicationDbContext.AnimalTypes.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<AnimalType?> GetByIdAsync(long id)
        {
            return await _applicationDbContext.AnimalTypes.FindAsync(id);
        }
    }
}
