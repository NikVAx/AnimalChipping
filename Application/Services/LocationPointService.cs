using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using System.Threading;

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

        public async Task<int> AddAsync(LocationPoint entity)
        {
            try
            {
                _applicationDbContext.LocationPoints.Add(entity);
                return await _applicationDbContext.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"Location point whit Latitude {entity.Latitude} and Longitude {entity.Longitude} is already exist", ex);
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
                throw new NotFoundException($"Location point with id {entity.Id} not found", ex);
            }
            catch(DbUpdateException ex)
            {
                throw new ConflictException($"Location point whit Latitude {entity.Latitude} and Longitude {entity.Longitude} is already exist", ex);
            } 
            catch (Exception ex)
            {
                throw new Exception("Rethrown exception", ex);
            }     
        }

        public async Task<int> RemoveAsync(long id)
        {
            try
            {
                LocationPoint entity = new() { Id = id };

                _applicationDbContext.LocationPoints
                    .Remove(entity);

                return await _applicationDbContext
                    .SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new NotFoundException($"Location point with id {id} not found", ex);
            }
        }
    }
}
