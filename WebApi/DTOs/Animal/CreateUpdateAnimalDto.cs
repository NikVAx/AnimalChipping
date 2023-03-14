using Domain.Enums;

namespace WebApi.DTOs.Animal
{
    public class CreateUpdateAnimalDto
    {
        public IEnumerable<long> AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public Gender Gender { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public int ChipperId { get; set; }
        public long ChippingLocationId { get; set; }
    }
}
