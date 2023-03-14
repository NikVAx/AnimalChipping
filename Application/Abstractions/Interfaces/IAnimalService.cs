using Application.DTOs;
using Domain.Entities;

namespace Application.Abstractions.Interfaces
{
    public interface IAnimalService :
        ICrudService<Animal, long>
    {
        public Task<IEnumerable<Animal>> SearchAsync(AnimalFilter options, int from = 0, int size = 10);

        public Task<int> AddAnimalType(long animalId, long animalType);
        public Task<int> UpdateAnimalType(long animalId, long oldTypeId, long newTypeId);
        public Task<int> RemoveAnimalType(long animalId, long animalType);
    }
}
