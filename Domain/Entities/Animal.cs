using Domain.Enums;

namespace Domain.Entities
{
    public class Animal
    {
        public long Id { get; set; }
        public long ChippingLocationId { get; set; }
        public int ChipperId { get; set; }
        public ICollection<AnimalType> AnimalTypes { get; set; }
        public ICollection<AnimalVisitedLocation> VisitedLocations { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public Gender Gender { get; set; }
        public LifeStatus LifeStatus { get; set; }
        public DateTimeOffset ChippingDateTime { get; set; }
        public DateTimeOffset? DeathDateTime { get; set; } = null;
    }
}