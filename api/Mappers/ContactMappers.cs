using api.Models;
using api.Utils;

namespace api.Mappers
{
    /// <summary>
    /// Mapper for Contact objects.
    /// </summary>
    /// <remarks>
    /// This class contains extension methods for mapping between Contact and ContactDTO objects.
    /// </remarks>
    public static class ContactMappers
    {
        /// <summary>
        /// Converts a Contact object to a ContactDTO object.
        /// </summary>
        /// <param name="contact">The Contact object to convert.</param>
        /// <returns>The converted ContactDTO object.</returns>
        public static ContactDTO ToContactDto(this Contact contact) => new()
        {
            Id = contact.Id,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Email = contact.Email,
            Phone = contact.Phone,
            BirthDate = contact.BirthDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            Category = contact.Category,
            CreatedAt = contact.CreatedAt,
            Registered = contact.Registered,
        };

        /// <summary>
        /// Converts a CreateContactDTO object to a Contact object.
        /// </summary>
        /// <param name="contactDto">The CreateContactDTO object to convert.</param>
        /// <returns>The converted Contact object.</returns>
        public static Contact ToContact(this CreateContactDTO contactDto) => new()
        {
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            Email = contactDto.Email,
            Phone = contactDto.Phone,
            BirthDate = DateOnly.FromDateTime(contactDto.BirthDate.SetKindUtc()),
            Category = contactDto.Category.ToContactCategory(),
        };

        /// <summary>
        /// Converts a RegisterDTO object to a Contact object.
        /// </summary>
        /// <param name="registerDTO">The RegisterDTO object to convert.</param>
        /// <returns>The converted Contact object.</returns>
        public static Contact ToContact(this RegisterDTO registerDTO) => new()
        {
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName,
            Email = registerDTO.Email,
            Phone = registerDTO.Phone,
            BirthDate = DateOnly.FromDateTime(registerDTO.BirthDate.SetKindUtc()),
            Category = registerDTO.Category.ToContactCategory(),
        };
    }
}