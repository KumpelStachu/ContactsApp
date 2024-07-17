using api.Data;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactCategoryController(ContactCategoryService categoryService) : Controller
    {
        private readonly ContactCategoryService _categoryService = categoryService;

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<ContactCategory>>> GetAll(int pageIndex = 1, int pageSize = 10)
        {
            return await _categoryService.GetCategories(pageIndex, pageSize);
        }

        [HttpGet("search")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<ContactCategory>>> Search([FromQuery] string name = "", [FromQuery] CategoryType type = CategoryType.OTHER, int pageIndex = 1, int pageSize = 10)
        {
            return await _categoryService.FindCategories(name, type, pageIndex, pageSize);
        }
    }
}