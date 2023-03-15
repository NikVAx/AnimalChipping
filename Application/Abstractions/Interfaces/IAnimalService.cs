using Application.DTOs;
using Domain.Entities;

namespace Application.Abstractions.Interfaces
{
    public interface IAnimalService :
        ICrudService<Animal, long>
    {
        public Task<IEnumerable<Animal>> SearchAsync(AnimalFilter options, int from = 0, int size = 10);

        public Task AddAnimalType(long animalId, long animalTypeId);
        public Task UpdateAnimalType(long animalId, long oldTypeId, long newTypeId);
        public Task RemoveAnimalType(long animalId, long animalType);
    }
}
