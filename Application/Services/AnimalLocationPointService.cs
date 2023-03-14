using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{

    public class AnimalLocationPointService :
        IAnimalLocationPointService
    {

        private readonly IApplicationDbContext _applicationDbContext;

        public AnimalLocationPointService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<AnimalVisitedLocation>> SearchAsync(
            long animalId,
            LocationFilter filter,
            int from = 0,
            int size = 10)
        {
            var query = _applicationDbContext.AnimalVisitedLocations
                .Where(x => x.AnimalId == animalId);
            if(filter.StartDateTime != null)
                query = query.Where(x => x.DateTimeOfVisitLocationPoint >= filter.StartDateTime);
            if(filter.EndDateTime != null)
                query = query.Where(x => x.DateTimeOfVisitLocationPoint <= filter.EndDateTime);

            return await query.Skip(from).Take(size)
                .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                .ToListAsync();
        }

        public async Task<AnimalVisitedLocation> AddAsync(long animalId, long pointId)
        {
            var animal = await _applicationDbContext.Animals
                .Include(x => x.VisitedLocations)
                .FirstOrDefaultAsync(x => x.Id == animalId);

            var point = await _applicationDbContext.LocationPoints
                .FirstOrDefaultAsync(x => x.Id == pointId);

            if (animal == null)
                throw new NotFoundException($"Animal with Id {animalId} is not found");
            
            if (point == null)
                throw new NotFoundException($"LocationPoint with Id {pointId} is not found");
            
            if (animal.LifeStatus == LifeStatus.DEAD)
                throw new OperationException("The Animal is dead, moving is impossible");

            if(animal.HasVisitedLocations())
            {
                var lastVisitedLocation = animal.VisitedLocations
                    .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                    .Last();

                if(lastVisitedLocation.LocationPointId == pointId)
                    throw new OperationException("The last VisitedLocatioPoint and new VisitedLocationPoint is equal");
            }
            else
            {
                if(pointId == animal.ChippingLocationId)
                    throw new OperationException("ChippingLocationPoint and added LocationPoint is equal");
            }

            var location = new AnimalVisitedLocation()
            {
                LocationPointId = pointId,
                AnimalId = animalId,
                DateTimeOfVisitLocationPoint = DateTimeOffset.UtcNow
            };

            animal.VisitedLocations
                .Add(location);

            await _applicationDbContext
                .SaveChangesAsync();

            return location;
        }

        public async Task<int> RemoveAsync(long animalId, long visitedPointId)
        {
            var animal = await _applicationDbContext.Animals
                .Include(x => x.VisitedLocations)
                .FirstOrDefaultAsync(x => x.Id == animalId);

            if(animal == null)
                throw new NotFoundException($"Animal with Id {animalId} is not found");

            var visitedLocationPoint = await _applicationDbContext.AnimalVisitedLocations
                .FirstOrDefaultAsync(x => x.Id == visitedPointId);

            if(visitedLocationPoint == null)
                throw new NotFoundException($"VisitedLocationPoint with Id {visitedPointId} is not found");

            if(animal.VisitedLocations.Any(x => x.Id == visitedPointId) == false)
                throw new NotFoundException($"Animal with Id {animalId} doesn't have VisitedLocationPoint with Id {visitedPointId}");

            var firstVisitedLocation = animal.VisitedLocations
                .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                .First();

            if(firstVisitedLocation.Id == visitedPointId)
            {
                animal.VisitedLocations.Remove(firstVisitedLocation);

                if(animal.HasVisitedLocations())
                {
                    var secondVisitedLocation = animal.VisitedLocations
                        .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                        .First();

                    if(secondVisitedLocation.LocationPointId == animal.ChippingLocationId)
                        animal.VisitedLocations.Remove(secondVisitedLocation);
                }
            }
            else
            {
                // TODO: Warning - it is possible that two consecutive points will be equal
                // 1 [0] - 2 [1] - 3 [2] - 2 [3] => remove 3 [2] =>  1 [0] - 2 [1] - 2 [2]

                var location = animal.VisitedLocations
                    .Where(x => x.Id == visitedPointId)
                    .First();

                animal.VisitedLocations.Remove(location);
            }

            return await _applicationDbContext.SaveChangesAsync();
        }


        public Task<int> UpdateAsync(long animalId, AnimalVisitedLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
