using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    /// <summary>
    /// Represents the request to authenticate a user.
    /// </summary>
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = true)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; } = null!;
    }
}