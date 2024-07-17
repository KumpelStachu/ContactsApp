using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    /// <summary>
    /// Represents a contact category to be created.
    /// </summary>
    public class CreateContactCategoryDTO
    {
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public CategoryType Type { get; set; }
    }
}