using api.Models;

namespace api.Mappers
{
    /// <summary>
    /// Mapper for ContactCategory objects.
    /// </summary>
    /// <remarks>
    /// This class contains extension methods for mapping between ContactCategory and ContactCategoryDTO objects.
    /// </remarks>
    public static class ContactCategoryMapper
    {
        /// <summary>
        /// Converts a ContactCategory object to a ContactCategoryDTO object.
        /// </summary>
        /// <param name="category">The ContactCategory object to convert.</param>
        /// <returns>The converted ContactCategoryDTO object.</returns>
        public static ContactCategoryDTO ToContactCategoryDTO(this ContactCategory category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
        };

        /// <summary>
        /// Converts a CreateContactCategoryDTO object to a ContactCategory object.
        /// </summary>
        /// <param name="categoryDto">The CreateContactCategoryDTO object to convert.</param>
        /// <returns>The converted ContactCategory object.</returns>
        public static ContactCategory ToContactCategory(this CreateContactCategoryDTO categoryDto) => new()
        {
            Name = categoryDto.Name,
            Type = categoryDto.Type,
        };

        /// <summary>
        /// Converts a ContactCategoryDTO object to a CreateContactCategoryDTO object.
        /// </summary>
        /// <param name="categoryDto">The ContactCategoryDTO object to convert.</param>
        /// <returns>The converted CreateContactCategoryDTO object.</returns>
        public static CreateContactCategoryDTO ToCreateContactCategoryDTO(this ContactCategoryDTO categoryDto) => new()
        {
            Name = categoryDto.Name,
            Type = categoryDto.Type,
        };
    }
}