﻿using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AnimalService :
        IAnimalService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AnimalService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<int> CreateAsync(Animal entity)
        {
            try
            {
                var types = await LoadTypes(entity);

                entity.AnimalTypes = types.ToArray();
                entity.ChippingDateTime = DateTimeOffset.UtcNow;

                _applicationDbContext.Animals
                    .Add(entity);

                int count = await _applicationDbContext
                    .SaveChangesAsync();

                _applicationDbContext.Animals
                    .Entry(entity).State = EntityState.Detached;

                return count;
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

        public async Task<int> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);

            if(entity == null)
                throw new NotFoundException($"Animal with Id '{id}' is not found");

            if(entity.VisitedLocations.Count > 0)
                throw new OperationException($"Animal already leave ChippingLocationPoint");

            _applicationDbContext.Animals
                .Remove(entity);
            
            return await _applicationDbContext
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

        public async Task<int> UpdateAsync(Animal entity)
        {
            try
            {
                var animal = await _applicationDbContext.Animals
                    .Include(x => x.VisitedLocations)
                    .FirstOrDefaultAsync(x => x.Id == entity.Id);

                if(animal == null)
                    throw new NotFoundException($"Animal with Id '{entity.Id}' is not found");

                if(animal.LifeStatus != entity.LifeStatus)
                {
                    if(animal.LifeStatus == LifeStatus.DEAD && entity.LifeStatus == LifeStatus.ALIVE)
                        throw new OperationException("Unable to change Animal LifeStatus 'DEAD' to 'ALIVE'");
                    else
                    {
                        animal.DeathDateTime = DateTimeOffset.UtcNow;
                        animal.LifeStatus = LifeStatus.DEAD;
                    }
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
                        throw new OperationException("ChippingLocationId and VisitedLocationPoint is equal");

                }

                _applicationDbContext.Animals
                    .Update(animal);

                return await _applicationDbContext
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
                    throw new NotFoundException($"AnimalType with Id {type.Id} is not found");

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

            if(animal == null)
                throw new NotFoundException("Animal not found");
            if(type == null)
                throw new NotFoundException("AnimalType not found");
            if(animal.AnimalTypes.Contains(type))
                throw new ConflictException($"Animal with Id {animal.Id} already has Type with Id {type.Id}");

            animal.AnimalTypes
                .Add(type);

            _applicationDbContext.Animals
                .Update(animal);

            await _applicationDbContext
                .SaveChangesAsync();
        }

        public Task UpdateAnimalType(long animalId, long oldTypeId, long newTypeId)
        {
            

            

            


            throw new NotImplementedException();
        }

        public Task RemoveAnimalType(long animalId, long animalType)
        {
            throw new NotImplementedException();
        }
    }
}
