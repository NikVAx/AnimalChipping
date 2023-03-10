using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// TODO: Set default FromQueryAttribute policy to convert Name from PascalCase to camelCase

namespace WebApi.DTOs.Animal
{
    public class AnimalFilterDto
    {
        [FromQuery(Name = "startDateTime")]
        public DateTime? StartDateTime { get; set; }

        [FromQuery(Name = "endDateTime")]
        public DateTime? EndDateTime { get; set; }

        [FromQuery(Name = "chipperId")]
        public int? ChipperId { get; set; }

        [FromQuery(Name = "chippingLocationId")]
        public long? ChippingLocationId { get; set; }

        [FromQuery(Name = "lifeStatus")]
        public LifeStatus? LifeStatus { get; set; }

        [FromQuery(Name = "gender")]
        public Gender? Gender { get; set; }
    }
}
