using api.Models;
using api.Utils;

namespace api.Mappers
{
    public static class ContactMappers
    {
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

        public static Contact ToContact(this CreateContactDTO contactDto) => new()
        {
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            Email = contactDto.Email,
            Phone = contactDto.Phone,
            BirthDate = DateOnly.FromDateTime(contactDto.BirthDate.SetKindUtc()),
            Category = contactDto.Category.ToContactCategory(),
        };

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