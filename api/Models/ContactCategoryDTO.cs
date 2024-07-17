using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ContactCategoryDTO
    {
        [Required]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public CategoryType Type { get; set; }
    }
}