namespace WebApi.DTOs.VisitedLocationPoint
{
    public class GetVisitedLocationPointDto
    {
        public long Id { get; set; }
        
        public DateTimeOffset DateTimeOfVisitLocationPoint { get; set; }
        
        public long LocationPointId { get; set; }
    }
}
