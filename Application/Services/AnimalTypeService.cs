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

        public async Task CreateAsync(AnimalType entity)
        {
            try
            {
                _applicationDbContext.AnimalTypes
                    .Add(entity);

                await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException($"AnimalType with Type {entity.Type} is already exist", ex);
            }
        }

        public async Task UpdateAsync(AnimalType entity)
        {
            try
            {
                _applicationDbContext.AnimalTypes
                    .Update(entity);

                await _applicationDbContext
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

        public async Task DeleteAsync(long id)
        {
            var type = await _applicationDbContext.AnimalTypes
                .Include(x => x.Animals)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(type == null)
                throw new NotFoundException($"AnimalType with Id {id} not found");

            if(type.Animals.Count > 0)
                throw new OperationException($"AnimalType with Id {id} have related Animals");

            _applicationDbContext.AnimalTypes
                .Remove(type);

            await _applicationDbContext
                .SaveChangesAsync();
        }
    }
}
