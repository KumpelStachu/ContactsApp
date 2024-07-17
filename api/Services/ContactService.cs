using System.Data;
using api.Data;
using api.Mappers;
using api.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ContactService(AppDbContext context, ContactCategoryService categoryService)
    {
        private readonly AppDbContext _context = context;
        private readonly ContactCategoryService _categoryService = categoryService;

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

        public async Task<Contact?> GetContact(int id)
        {
            return await _context.Contacts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Contact?> GetContact(string email)
        {
            return await _context.Contacts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Email == email);
        }

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

        public async Task DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id) ?? throw new KeyNotFoundException(); ;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        public bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }

        public bool ContactExists(string email)
        {
            return _context.Contacts.Any(e => e.Email == email);
        }
    }
}