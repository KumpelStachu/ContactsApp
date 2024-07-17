using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models
{
    /// <summary>
    /// Represents a contact category.
    /// </summary>
    public class ContactCategory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public CategoryType Type { get; set; }

        public bool Equals(CreateContactCategoryDTO categoryDTO)
        {
            return Name == categoryDTO.Name && Type.Equals(categoryDTO.Type);
        }
    }

    /// <summary>
    /// Represents a contact category type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoryType
    {
        PERSONAL,
        WORK,
        OTHER,
    }
}