using Domain.Enums;

namespace Application.DTOs
{
    public class AnimalFilter
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? ChipperId { get; set; }
        public long? ChippingLocationId { get; set; }
        public LifeStatus? LifeStatus { get; set; }
        public Gender? Gender { get; set; }
    }
}
