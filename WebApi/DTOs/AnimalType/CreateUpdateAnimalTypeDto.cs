using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.AnimalType
{
    public class CreateUpdateAnimalTypeDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; } = null!;
    }
}