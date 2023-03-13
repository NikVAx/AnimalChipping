using Application.Abstractions.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<AnimalType?> GetByIdAsync(long id)
        {
            return await _applicationDbContext.AnimalTypes
                .FindAsync(id);
        }

        public async Task<int> CreateAsync(AnimalType entity)
        {
            try
            {
                _applicationDbContext.AnimalTypes
                    .Add(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException($"AnimalType with Type {entity.Type} is already exist", ex);
            }
        }

        public async Task<int> UpdateAsync(AnimalType entity)
        {
            try
            {
                _applicationDbContext.AnimalTypes
                    .Update(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new NotFoundException($"AnimalType with Id {entity.Id} not found", ex);
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"AnimalType with Type {entity.Type} is already exist", ex);
            }
        }

        public async Task<int> DeleteAsync(long id)
        {
            try
            {
                AnimalType entity = new() { Id = id };

                _applicationDbContext.AnimalTypes
                    .Remove(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new NotFoundException($"AnimalType with Id {id} not found", ex);
            }
        }
    }
}
