using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class LoginDTO
    {
        [Required(AllowEmptyStrings = true)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; } = null!;
    }
}