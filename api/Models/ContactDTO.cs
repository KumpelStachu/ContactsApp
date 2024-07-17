using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ContactDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public ContactCategory Category { get; set; } = new();
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool Registered { get; set; }
    }
}