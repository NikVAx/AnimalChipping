using Microsoft.AspNetCore.Mvc;

namespace WebApi.DTOs.VisitedLocationPoint
{
    public class LocationFilterDto
    {
        [FromQuery(Name = "startDateTime")]
        public DateTime? StartDateTime { get; set; }
        [FromQuery(Name = "endDateTime")]
        public DateTime? EndDateTime { get; set; }
    }
}
