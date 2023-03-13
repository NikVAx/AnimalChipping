namespace WebApi.DTOs.VisitedLocationPoint
{
    public class LocationFilterDto
    {
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
    }
}
