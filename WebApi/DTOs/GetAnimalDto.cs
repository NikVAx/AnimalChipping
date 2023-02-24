namespace WebApi.DTOs
{
    public class GetAnimalDto
    {
        public long Id { get; set; }
        public IEnumerable<long> AnimalTypes { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; }
        public string LifeStatus { get; set; }
        public DateTime ChippingDateTime { get; set; }
        public int ChipperId { get; set; }
        public long ChippingLocationId { get; set; }
        public IEnumerable<long> VisitedLocation { get; set; }
        public DateTime? DeathDateTime { get; set; }
    }
}
