using Application.DTOs;
using Domain.Entities;

namespace Application.Abstractions.Interfaces
{
    public interface IAnimalService :
        ICrudService<Animal, long>
    {
        public Task<IEnumerable<Animal>> SearchAsync(AnimalFilter options, int from = 0, int size = 10);

        public Task<IEnumerable<AnimalVisitedLocation>> SearchAnimalVisitedLocationsAsync(long animalId, 
                LocationFilter filter, int from = 0, int size = 10);
    }
}
