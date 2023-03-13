using Application.Abstractions.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<int> AddAsync(long animalId, long pointId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(long animalId, long pointId)
        {
            throw new NotImplementedException();
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
                query.Where(x => x.DateTimeOfVisitLocationPoint == filter.StartDateTime);
            if(filter.EndDateTime != null)
                query.Where(x => x.DateTimeOfVisitLocationPoint == filter.EndDateTime);

            return await query.Skip(from).Take(size)
                .OrderBy(x => x.DateTimeOfVisitLocationPoint)
                .ToListAsync();
        }

        public Task<int> UpdateAsync(long animalId, AnimalVisitedLocation location)
        {
            throw new NotImplementedException();
        }
    }
}
