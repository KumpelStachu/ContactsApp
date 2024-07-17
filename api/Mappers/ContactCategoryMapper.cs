using api.Models;

namespace api.Mappers
{
    public static class ContactCategoryMapper
    {
        public static ContactCategoryDTO ToContactCategoryDTO(this ContactCategory category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
        };

        public static ContactCategory ToContactCategory(this CreateContactCategoryDTO categoryDto) => new()
        {
            Name = categoryDto.Name,
            Type = categoryDto.Type,
        };

        public static CreateContactCategoryDTO ToCreateContactCategoryDTO(this ContactCategoryDTO categoryDto) => new()
        {
            Name = categoryDto.Name,
            Type = categoryDto.Type,
        };
    }
}