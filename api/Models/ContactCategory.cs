using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models
{
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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoryType
    {
        PERSONAL,
        WORK,
        OTHER,
    }
}