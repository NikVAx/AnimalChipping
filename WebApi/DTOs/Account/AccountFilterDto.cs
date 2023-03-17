using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Account
{
    public class AccountFilterDto
    {
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        public string? Email { get; set; }
    }
}
