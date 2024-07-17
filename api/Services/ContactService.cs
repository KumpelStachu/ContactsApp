using System.Data;
using api.Data;
using api.Mappers;
using api.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    /// <summary>
    /// Service for managing contacts.
    /// </summary>
    /// <remarks>
    /// This class contains methods for retrieving, creating, updating, and deleting contacts.
    /// </remarks>
    public class ContactService(AppDbContext context, ContactCategoryService categoryService)
    {
        private readonly AppDbContext _context = context;
        private readonly ContactCategoryService _categoryService = categoryService;

        /// <summary>
        /// Gets a paginated list of contacts.
        /// </summary>
        /// <param name="pageIndex">The index of the page to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paginated list of contacts.</returns>
        public async Task<PaginatedList<Contact>> GetContacts(int pageIndex, int pageSize)
        {
            var contacts = await _context.Contacts
                .Include(p => p.Category)
                .OrderBy(c => c.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalContacts = await _context.Contacts.CountAsync();
            var totalPages = (int)Math.Ceiling(totalContacts / (double)pageSize);

            return new PaginatedList<Contact>(contacts, pageIndex, totalPages);
        }

        /// <summary>
        /// Gets a contact by ID.
        /// </summary>
        /// <param name="id">The ID of the contact to get.</param>
        /// <returns>The contact with the specified ID.</returns>
        /// <remarks>
        /// If the contact does not exist, null is returned.
        /// </remarks>
        public async Task<Contact?> GetContact(int id)
        {
            return await _context.Contacts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Gets a contact by email.
        /// </summary>
        /// <param name="email">The email of the contact to get.</param>
        /// <returns>The contact with the specified email.</returns>
        /// <remarks>
        /// If the contact does not exist, null is returned.
        /// </remarks>
        public async Task<Contact?> GetContact(string email)
        {
            return await _context.Contacts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Email == email);
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="registerDTO">The registration data.</param>
        /// <returns>The created user.</returns>
        /// <exception cref="DBConcurrencyException">Thrown when the user already exists.</exception>
        /// <remarks>
        /// If the contact already exists, the user is updated.
        /// </remarks>
        public async Task<Contact> CreateUser(RegisterDTO registerDTO)
        {
            var contact = await GetContact(registerDTO.Email);
            if (contact?.Password != null)
            {
                throw new DBConcurrencyException();
            }

            var user = registerDTO.ToContact();
            user.Category = await _categoryService.FindOrCreateCategory(registerDTO.Category);
            user.Password = Argon2.Hash(registerDTO.Password);

            if (contact != null)
            {
                user.Id = contact.Id;
                _context.Entry(contact).CurrentValues.SetValues(user);
            }
            else
            {
                _context.Contacts.Add(user);
            }
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Creates a contact.
        /// </summary>
        /// <param name="contactDTO">The data for the contact to create.</param>
        /// <returns>The created contact.</returns>
        /// <exception cref="DBConcurrencyException">Thrown when the contact already exists.</exception>
        public async Task<Contact> CreateContact(CreateContactDTO contactDTO)
        {
            if (ContactExists(contactDTO.Email))
            {
                throw new DBConcurrencyException();
            }

            var contact = contactDTO.ToContact();
            contact.Category = await _categoryService.FindOrCreateCategory(contactDTO.Category);

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="contactDTO">The data for the contact to update.</param>
        /// <returns>The updated contact.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the contact is not found.</exception>
        /// <exception cref="DBConcurrencyException">Thrown when the contact already exists.</exception>
        public async Task<Contact?> UpdateContact(UpdateContactDTO contactDTO)
        {
            var contact = await _context.Contacts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == contactDTO.Id)
                ?? throw new KeyNotFoundException();

            if (contact.Email != contactDTO.Email && ContactExists(contactDTO.Email))
            {
                throw new DBConcurrencyException();
            }

            contact.FirstName = contactDTO.FirstName;
            contact.LastName = contactDTO.LastName;
            contact.Email = contactDTO.Email;
            contact.Phone = contactDTO.Phone;
            contact.BirthDate = DateOnly.FromDateTime(contactDTO.BirthDate);

            if (!contact.Category.Equals(contactDTO.Category))
            {
                contact.Category = await _categoryService.FindOrCreateCategory(contactDTO.Category);
            }

            await _context.SaveChangesAsync();

            return contact;
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the contact is not found.</exception>
        public async Task DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id) ?? throw new KeyNotFoundException(); ;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a contact exists.
        /// </summary>
        /// <param name="email">The email of the contact to check.</param>
        /// <returns>True if the contact exists, false otherwise.</returns>
        public bool ContactExists(string email)
        {
            return _context.Contacts.Any(e => e.Email == email);
        }
    }
}