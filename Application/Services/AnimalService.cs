using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Exceptions.BaseLogicExceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AnimalService :  IAnimalService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AnimalService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task CreateAsync(Animal entity)
        {
            try
            {
                var types = await LoadTypes(entity);
                
                entity.AnimalTypes = types.ToArray();
                entity.ChippingDateTime = DateTimeOffset.UtcNow;

                _applicationDbContext.Animals
                    .Add(entity);

                await _applicationDbContext
                    .SaveChangesAsync();

                _applicationDbContext.Animals
                    .Entry(entity).State = EntityState.Detached;
            }
            catch (DbUpdateException ex)
            {
                throw new NotFoundException("Used Account or LocationPoint is not found", ex);
            }
        }

        public async Task<Animal?> GetByIdAsync(long id)
        {
            var animal = await _applicationDbContext.Animals
                .Where(x => x.Id == id)
                .Include(x => x.AnimalTypes)
                .Include(x => x.VisitedLocations)
                .FirstOrDefaultAsync();

            return animal;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);

            if(entity == null)
                throw new NotFoundException(typeof(Animal), id);

            if(entity.VisitedLocations.Count > 0)
                throw new InvalidDomainOperationException($"Animal already leave ChippingLocationPoint");

            _applicationDbContext.Animals
                .Remove(entity);
            
            await _applicationDbContext
                .SaveChangesAsync();
        }

        public async Task<IEnumerable<Animal>> SearchAsync(
            AnimalFilter options,
            int from = 0,
            int size = 10)
        {
            var query = _applicationDbContext.Animals.AsQueryable();

            if(options.StartDateTime != null)
                query = query.Where(x => x.ChippingDateTime >= options.StartDateTime);
            if(options.EndDateTime != null)
                query = query.Where(x => x.ChippingDateTime <= options.EndDateTime);
            if(options.ChipperId != null)
                query = query.Where(x => x.ChipperId == options.ChipperId);
            if(options.ChippingLocationId != null)
                query = query.Where(x => x.ChippingLocationId == options.ChippingLocationId);
            if(options.LifeStatus != null)
                query = query.Where(x => x.LifeStatus == options.LifeStatus);
            if(options.Gender != null)
                query = query.Where(x => x.Gender == options.Gender);

            return await query
                .Skip(from)
                .Take(size)
                .Include(x => x.AnimalTypes)
                .Include(x => x.VisitedLocations)
                .ToListAsync();
        }

        public async Task UpdateAsync(Animal entity)
        {
            try
            {
                var animal = await _applicationDbContext.Animals
                    .Include(x => x.VisitedLocations)
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

                if(animal == null)
                    throw new NotFoundException(typeof(Animal), entity.Id);

                if(animal.LifeStatus != entity.LifeStatus)
                {
                    if(animal.LifeStatus == LifeStatus.DEAD && entity.LifeStatus == LifeStatus.ALIVE)
                        throw new InvalidDomainOperationException("Unable to change Animal LifeStatus 'DEAD' to 'ALIVE'");
                    
                    animal.DeathDateTime = DateTimeOffset.UtcNow;
                    animal.LifeStatus = LifeStatus.DEAD;
                }

                animal.Weight = entity.Weight;
                animal.Height = entity.Height;
                animal.Length = entity.Length;
                animal.ChipperId = entity.ChipperId;
                animal.ChippingLocationId = entity.ChippingLocationId;
                animal.Gender = entity.Gender;

                if(animal.HasVisitedLocations())
                {
                    var firstVisitedLocation = animal.VisitedLocations
                        .First();

                    if (firstVisitedLocation.LocationPointId == animal.ChippingLocationId)
                        throw new InvalidDomainOperationException("ChippingLocationId and VisitedLocationPoint is equal");
                }

                _applicationDbContext.Animals
                    .Update(animal);

                await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new NotFoundException("LocationPoint or Account is not found", ex);
            }
        }

        private async Task<IEnumerable<AnimalType>> LoadTypes(Animal entity)
        {
            var types = new List<AnimalType>();

            foreach(var type in entity.AnimalTypes)
            {
                var found = await _applicationDbContext.AnimalTypes
                    .FindAsync(type.Id);

                if(found == null)
                    throw new NotFoundException(typeof(AnimalType), type.Id);

                types.Add(found);
            }

            return types;
        }

        public async Task AddAnimalType(long animalId, long animalTypeId)
        {
            var animal = await _applicationDbContext.Animals
                .Include(x => x.AnimalTypes)
                .FirstOrDefaultAsync(x => x.Id == animalId);

            var type = await _applicationDbContext.AnimalTypes
                .FirstOrDefaultAsync(x => x.Id == animalTypeId);

            if (animal == null)
                throw new NotFoundException(typeof(Animal), animalId);

            if(type == null)
                throw new NotFoundException(typeof(AnimalType), animalTypeId);

            if(animal.AnimalTypes.Contains(type))
                throw new ConflictException($"Animal with Id {animalId} already has Type with Id {animalTypeId}");

            animal.AnimalTypes
                .Add(type);

            _applicationDbContext.Animals
                .Update(animal);

            await _applicationDbContext
                .SaveChangesAsync();
        }

        public async Task UpdateAnimalType(long animalId, long oldTypeId, long newTypeId)
        {
            var animal = await _applicationDbContext.Animals
                .Include(x => x.AnimalTypes)
                .FirstOrDefaultAsync(x => x.Id == animalId);

            var newType = await _applicationDbContext.AnimalTypes
                .FirstOrDefaultAsync(x => x.Id == newTypeId);

            var oldType = await _applicationDbContext.AnimalTypes
                .FirstOrDefaultAsync(x => x.Id == oldTypeId);

            if (animal == null)
                throw new NotFoundException(typeof(Animal), animalId);
            
            if(oldType == null)
                throw new NotFoundException(typeof(AnimalType), oldTypeId);

            if (newType == null)
                throw new NotFoundException(typeof(AnimalType), newTypeId);

            if (animal.AnimalTypes.Contains(oldType) == false)
                throw new NotFoundException(typeof(AnimalType), oldTypeId,
                    typeof(Animal), animalId);
            
            if(animal.AnimalTypes.Contains(newType))
                throw new ConflictException($"Animal with Id {animalId} already has Type with Id {newTypeId}");

            animal.AnimalTypes
                .Remove(oldType);
            animal.AnimalTypes
                .Add(newType);
            _applicationDbContext.Animals
                .Update(animal);

            await _applicationDbContext
                .SaveChangesAsync();
        }

        public async Task RemoveAnimalType(long animalId, long animalTypeId)
        {
            var animal = await _applicationDbContext.Animals
                .Include(x => x.AnimalTypes)
                .FirstOrDefaultAsync(x => x.Id == animalId);

            var type = await _applicationDbContext.AnimalTypes
                .FirstOrDefaultAsync(x => x.Id == animalTypeId);

            if(animal == null)
                throw new NotFoundException(typeof(Animal), animalId);
            
            if(type == null)
                throw new NotFoundException(typeof(AnimalType), animalTypeId);

            if (animal.AnimalTypes.Contains(type) == false)
                throw new NotFoundException(typeof(AnimalType), type.Id, typeof(Animal),
                    animal.Id);

            if(animal.AnimalTypes.Count == 1)
                throw new InvalidDomainOperationException("Only AnimalType can't be removed");

            animal.AnimalTypes
                .Remove(type);

            _applicationDbContext.Animals
                .Update(animal);

            await _applicationDbContext
                .SaveChangesAsync();
        }
    }
}
