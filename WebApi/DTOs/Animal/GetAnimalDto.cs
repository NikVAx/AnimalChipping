using Domain.Enums;

namespace WebApi.DTOs.Animal
{
    public class GetAnimalDto
    {
        public long Id { get; set; }
        public long ChippingLocationId { get; set; }
        public int ChipperId { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public IEnumerable<long> AnimalTypes { get; set; }
        public IEnumerable<long> VisitedLocations { get; set; }
        public Gender Gender { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public DateTimeOffset ChippingDateTime { get; set; }
        public DateTimeOffset? DeathDateTime { get; set; }
    }
}
