using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Interfaces
{
    public interface IAnimalLocationPointService
    {
        public Task<IEnumerable<AnimalVisitedLocation>> SearchAsync(long animalId,
            LocationFilter filter, int from = 0, int size = 10);

        public Task<AnimalVisitedLocation> AddAsync(long animalId, long pointId);
        public Task<AnimalVisitedLocation> UpdateAsync(long animalId, long visitedLocationPointId, long locationPointId);
        public Task RemoveAsync(long animalId, long pointId);

    }
}
