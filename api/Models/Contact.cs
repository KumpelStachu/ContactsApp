using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using api.Utils;

namespace api.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; } = null;
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly BirthDate { get; set; }
        [Required]
        public ContactCategory Category { get; set; } = new();
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool Registered => Password != null;
    }
}