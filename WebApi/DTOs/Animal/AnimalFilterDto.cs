using Domain.Enums;

namespace WebApi.DTOs.Animal
{
    public class AnimalFilterDto
    {
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public int? ChipperId { get; set; }
        public long? ChippingLocationId { get; set; }
        public LifeStatus? LifeStatus { get; set; }
        public Gender? Gender { get; set; }
    }
}
