using WebApi.Attibutes.ValidationAttibutes;

namespace WebApi.DTOs.VisitedLocationPoint
{
    public class UpdateVisitedLocationPointDto
    {
        [MinInt64(1)]
        public long VisitedLocationPointId { get; set; }
        
        [MinInt64(1)]
        public long LocationPointId { get; set; }
    }
}
