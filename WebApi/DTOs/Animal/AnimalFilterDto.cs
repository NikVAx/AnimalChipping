using Domain.Enums;
using WebApi.Attibutes.ValidationAttibutes;

namespace WebApi.DTOs.Animal
{
    public class AnimalFilterDto
    {
        public DateTimeOffset? StartDateTime { get; set; }
        
        public DateTimeOffset? EndDateTime { get; set; }

        [MinInt32(1)]
        public int? ChipperId { get; set; }
        
        [MinInt64(1)]
        public long? ChippingLocationId { get; set; }

        public LifeStatus? LifeStatus { get; set; }
        
        public Gender? Gender { get; set; }
    }
}
