using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    /// <summary>
    /// Represents a contact to be updated.
    /// </summary>
    public class UpdateContactDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = true)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = true)]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public CreateContactCategoryDTO Category { get; set; } = new();
    }
}