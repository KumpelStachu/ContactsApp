using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    /// <summary>
    /// Represents the response to an authentication request.
    /// </summary>
    public class AuthResponse
    {
        [Required]
        public string Token { get; set; } = null!;
        [Required]
        public ContactDTO User { get; set; } = null!;
    }
}