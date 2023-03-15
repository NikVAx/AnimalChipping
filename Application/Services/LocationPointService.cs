using Application.Abstractions.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class LocationPointService :
        ILocationPointService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public LocationPointService(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<LocationPoint?> GetByIdAsync(long id)
        {
            return await _applicationDbContext.LocationPoints
                .FindAsync(id);
        }

        public async Task<int> CreateAsync(LocationPoint entity)
        {
            try
            {
                _applicationDbContext.LocationPoints
                    .Add(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"LocationPoint with Latitude {entity.Latitude} and Longitude {entity.Longitude} is already exist", ex);
            }
        }

        public async Task<int> UpdateAsync(LocationPoint entity)
        {
            try
            {
                _applicationDbContext.LocationPoints
                    .Update(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new NotFoundException($"LocationPoint with Id {entity.Id} not found", ex);
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"LocationPoint with Latitude {entity.Latitude} and Longitude {entity.Longitude} is already exist", ex);
            }  
        }

        public async Task<int> DeleteAsync(long id)
        {
            try
            {
                var haveRelatedChippingLocations = await _applicationDbContext.Animals
                    .AnyAsync(x => x.ChippingLocationId == id);
                var haveRelatedVisitedLocations = await _applicationDbContext.AnimalVisitedLocations
                    .AnyAsync(x => x.LocationPointId == id);

                if(haveRelatedChippingLocations || haveRelatedVisitedLocations)
                    throw new OperationException("Location point have related entities");

                LocationPoint entity = new() { Id = id };

                _applicationDbContext.LocationPoints
                    .Remove(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new NotFoundException($"LocationPoint with Id {id} not found", ex);
            }
        }
    }
}
