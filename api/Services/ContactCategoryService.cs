using System.Data;
using api.Data;
using api.Exceptions;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ContactCategoryService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<PaginatedList<ContactCategory>> GetCategories(int pageIndex, int pageSize)
        {
            var categories = await _context.ContactCategories
                .OrderBy(b => b.Type)
                .OrderBy(b => b.Name)
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.ContactCategories.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<ContactCategory>(categories, pageIndex, totalPages);
        }

        public async Task<ContactCategory?> GetCategory(int id)
        {
            return await _context.ContactCategories.FindAsync(id);
        }

        public async Task<ContactCategory?> FindCategory(string name, CategoryType type)
        {
            return await _context.ContactCategories.FirstOrDefaultAsync(c => c.Name == name && c.Type == type);
        }

        public async Task<PaginatedList<ContactCategory>> FindCategories(string name, CategoryType type, int pageIndex, int pageSize)
        {
            var categories = await _context.ContactCategories
                .Where(c => c.Type == type && EF.Functions.ILike(c.Name, $"%{name}%"))
                .OrderBy(b => b.Name)
                .OrderBy(b => b.Type)
                .OrderBy(b => b.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await _context.ContactCategories.CountAsync(c => c.Type == type && EF.Functions.ILike(c.Name, $"%{name}%"));
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<ContactCategory>(categories, pageIndex, totalPages);
        }

        public async Task<ContactCategory> CreateCategory(CreateContactCategoryDTO categoryDTO)
        {
            if (await FindCategory(categoryDTO.Name, categoryDTO.Type) != null)
            {
                throw new DBConcurrencyException();
            }

            var category = categoryDTO.ToContactCategory();
            _context.ContactCategories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<ContactCategory> FindOrCreateCategory(CreateContactCategoryDTO categoryDTO)
        {
            var category = await FindCategory(categoryDTO.Name, categoryDTO.Type);
            category ??= await CreateCategory(categoryDTO);

            return category;
        }

        public bool CategoryExists(int id)
        {
            return _context.ContactCategories.Any(e => e.Id == id);
        }
    }
}