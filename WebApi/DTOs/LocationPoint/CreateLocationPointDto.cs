using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.LocationPoint
{
    public class CreateLocationPointDto
    {
        [Range(-90, 90)]
        [Required]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        [Required]
        public double Longitude { get; set; }
    }
}
