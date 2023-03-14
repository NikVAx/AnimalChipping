namespace Domain.Entities
{
    public class AnimalType
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public ICollection<Animal> Animals { get; set; }
    }
}