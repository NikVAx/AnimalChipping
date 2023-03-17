using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.LocationPoint
{
    public class CreateUpdateLocationPointDto
    {
        [Required, Range(-90.00, 90.00)]
        public double Latitude { get; set; }

        [Required, Range(-180.00, 180.00)]
        public double Longitude { get; set; }
    }
}
