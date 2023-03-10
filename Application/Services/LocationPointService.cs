using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
            return await _applicationDbContext.LocationPoints.FindAsync(id);
        }

        public async Task<int> AddAsync(LocationPoint entity)
        {
            _applicationDbContext.LocationPoints.Add(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(LocationPoint entity)
        {
            _applicationDbContext.LocationPoints.Update(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(LocationPoint entity)
        {
            _applicationDbContext.LocationPoints.Remove(entity);
            return await _applicationDbContext.SaveChangesAsync();
        }
    }
}
