using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class AuthResponse
    {
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public ContactDTO User { get; set; } = null!;
    }
}