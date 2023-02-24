namespace WebApi.DTOs
{
    public class GetVisitedLocationPointDto
    {
        public long Id { get; set; }
        public DateTime DateTimeOfVisitLocationPoint { get; set; }
        public long LocationPointId { get; set; }
    }
}
