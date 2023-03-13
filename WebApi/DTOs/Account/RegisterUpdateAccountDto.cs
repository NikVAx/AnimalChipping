using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Account
{
    public class RegisterUpdateAccountDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
