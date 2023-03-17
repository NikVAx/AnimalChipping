using Domain.Enums;
using WebApi.Attibutes.ValidationAttibutes;

namespace WebApi.DTOs.Animal
{
    public class CreateAnimalDto
    {
        public IEnumerable<long> AnimalTypes { get; set; }
        
        [MinSingle(0, includeMin: false)]
        public float Weight { get; set; }
        
        [MinSingle(0, includeMin: false)]
        public float Length { get; set; }
        
        [MinSingle(0, includeMin: false)]
        public float Height { get; set; }
        
        public Gender Gender { get; set; }
        
        public LifeStatus LifeStatus { get; set; }
        
        [MinInt32(1)]
        public int ChipperId { get; set; }
        
        [MinInt64(1)]
        public long ChippingLocationId { get; set; }
    }
}
