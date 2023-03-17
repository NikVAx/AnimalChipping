using WebApi.Attibutes.ValidationAttibutes;

namespace WebApi.DTOs.Animal
{
    public class EditAnimalTypeDto
    {
        [MinInt64(1)]
        public long OldTypeId { get; set; }

        [MinInt64(1)]
        public long NewTypeId { get; set; } 
    }
}
