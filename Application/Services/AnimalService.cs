using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
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

        public async Task<int> AddAsync(Animal entity)
        {
            _applicationDbContext.Animals.Add(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<Animal?> GetByIdAsync(long id)
            => await _applicationDbContext.Animals.FindAsync(id);

        public async Task<int> RemoveAsync(Animal entity)
        {
            _applicationDbContext.Animals.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AnimalVisitedLocation>> SearchAnimalVisitedLocationsAsync(
            long animalId,
            LocationFilter filter,
            int from = 0,
            int size = 10)
        {
            var query = _applicationDbContext.AnimalVisitedLocations
                .Where(x => x.AnimalId == animalId);
            if(filter.StartDateTime != null)
                query.Where(x => x.DateTimeOfVisitLocationPoint == filter.StartDateTime);
            if(filter.EndDateTime != null)
                query.Where(x => x.DateTimeOfVisitLocationPoint == filter.EndDateTime);

            return await query.Skip(from).Take(size)
                .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                .ToListAsync();
        }

        public async Task<IEnumerable<Animal>> SearchAsync(DateTime? startDateTime,
            DateTime? endDateTime, int? chipperId, long? chippingLocationId,
            LifeStatus? lifeStatus, Gender? gender, int from = 0, int size = 10)
        {
            var query = _applicationDbContext.Animals.AsQueryable();

            if(startDateTime != null)
                query.Where(x => x.ChippingDateTime >= startDateTime);
            if(endDateTime != null)
                query.Where(x => x.ChippingDateTime <= endDateTime);
            if(chipperId != null)
                query.Where(x => x.ChipperId == chipperId);
            if(chippingLocationId != null)
                query.Where(x => x.ChippingLocationId == chippingLocationId);
            if(lifeStatus != null)
                query.Where(x => x.LifeStatus == lifeStatus);
            if(gender != null)
                query.Where(x => x.Gender == gender);

            return await query.Skip(from).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Animal>> SearchAsync(AnimalFilter options, int from = 0, int size = 10)
        {
            var query = _applicationDbContext.Animals.AsQueryable();

            if(options.StartDateTime != null)
                query.Where(x => x.ChippingDateTime >= options.StartDateTime);
            if(options.EndDateTime != null)
                query.Where(x => x.ChippingDateTime <= options.EndDateTime);
            if(options.ChipperId != null)
                query.Where(x => x.ChipperId == options.ChipperId);
            if(options.ChippingLocationId != null)
                query.Where(x => x.ChippingLocationId == options.ChippingLocationId);
            if(options.LifeStatus != null)
                query.Where(x => x.LifeStatus == options.LifeStatus);
            if(options.Gender != null)
                query.Where(x => x.Gender == options.Gender);

            return await query.Skip(from).Take(size).ToListAsync();
        }

        public async Task<int> UpdateAsync(Animal entity)
        {
            _applicationDbContext.Animals.Update(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }
    }
}
