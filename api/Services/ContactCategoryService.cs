using System.Data;
using api.Data;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    /// <summary>
    /// Service for managing contact categories.
    /// </summary>
    /// <remarks>
    /// This class contains methods for retrieving, creating, and updating contact categories.
    /// </remarks>
    public class ContactCategoryService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Gets a paginated list of contact categories.
        /// </summary>
        /// <param name="pageIndex">The index of the page to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paginated list of contact categories.</returns>
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

        /// <summary>
        /// Gets a contact category by ID.
        /// </summary>
        /// <param name="id">The ID of the contact category to get.</param>
        /// <returns>The contact category with the specified ID.</returns>
        /// <remarks>
        /// If the contact category does not exist, null is returned.
        /// </remarks>
        public async Task<ContactCategory?> GetCategory(int id)
        {
            return await _context.ContactCategories.FindAsync(id);
        }

        /// <summary>
        /// Finds a contact category by name and type.
        /// </summary>
        /// <param name="name">The name of the contact category to find.</param>
        /// <param name="type">The type of the contact category to find.</param>
        /// <returns>The contact category with the specified name and type.</returns>
        /// <remarks>
        /// If the contact category does not exist, null is returned.
        /// </remarks>
        public async Task<ContactCategory?> FindCategory(string name, CategoryType type)
        {
            return await _context.ContactCategories.FirstOrDefaultAsync(c => c.Name == name && c.Type == type);
        }

        /// <summary>
        /// Finds contact categories by name and type.
        /// </summary>
        /// <param name="name">The name of the contact categories to find.</param>
        /// <param name="type">The type of the contact categories to find.</param>
        /// <param name="pageIndex">The index of the page to get.</param>
        /// <param name="pageSize">The size of the page to get.</param>
        /// <returns>The paginated list of contact categories with the specified name and type.</returns>
        /// <remarks>
        /// If no contact categories are found, an empty list is returned.
        /// </remarks>
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

        /// <summary>
        /// Creates a contact category.
        /// </summary>
        /// <param name="categoryDTO">The data for the contact category to create.</param>
        /// <returns>The created contact category.</returns>
        /// <exception cref="DBConcurrencyException">Thrown when the contact category already exists.</exception>
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

        /// <summary>
        /// Updates a contact category.
        /// </summary>
        /// <param name="id">The ID of the contact category to update.</param>
        /// <param name="categoryDTO">The data for the contact category to update.</param>
        /// <returns>The updated contact category.</returns>
        public async Task<ContactCategory> FindOrCreateCategory(CreateContactCategoryDTO categoryDTO)
        {
            var category = await FindCategory(categoryDTO.Name, categoryDTO.Type);
            category ??= await CreateCategory(categoryDTO);

            return category;
        }
    }
}