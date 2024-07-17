using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    /// <summary>
    /// Represents a contact to be created.
    /// </summary>
    public class CreateContactDTO
    {
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