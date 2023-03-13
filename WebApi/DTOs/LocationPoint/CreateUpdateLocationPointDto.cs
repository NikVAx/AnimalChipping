using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.LocationPoint
{
    public class CreateUpdateLocationPointDto
    {
        [Range(-90.00, 90.00)]
        [Required]
        public double Latitude { get; set; }

        [Range(-180.00, 180.00)]
        [Required]
        public double Longitude { get; set; }
    }
}
