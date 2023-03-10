using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApi.DTOs.Account
{
    public class AccountFilterDto
    {
        [FromQuery(Name = "firstName")]
        public string? FirstName { get; set; }
        
        [FromQuery(Name = "lastName")]
        public string? LastName { get; set; }

        [EmailAddress]
        [FromQuery(Name = "email")]
        public string? Email { get; set; }
    }
}
