using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApi.DTOs.Account
{
    public class AccountFilterDto
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
