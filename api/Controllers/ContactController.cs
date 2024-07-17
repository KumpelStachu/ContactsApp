using System.Data;
using System.Security.Claims;
using api.Mappers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactController(ContactService contactService) : ControllerBase
    {
        private readonly ContactService _contactService = contactService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<ContactDTO>>> GetContacts(int pageIndex = 1, int pageSize = 10)
        {
            var contacts = await _contactService.GetContacts(pageIndex, pageSize);
            return contacts.Select(c => c.ToContactDto());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<ContactDTO>> GetContact(int id)
        {
            var contact = await _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            return contact.ToContactDto();
        }

        [HttpGet("check")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDTO>> CheckEmail(string email)
        {
            var contact = await _contactService.GetContact(email);
            if (contact == null)
            {
                return NotFound();
            }

            return contact.ToContactDto();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ContactDTO>> CreateContact(CreateContactDTO contactDTO)
        {
            try
            {
                var contact = await _contactService.CreateContact(contactDTO);

                return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact.ToContactDto());
            }
            catch (DBConcurrencyException)
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateContact(int id, UpdateContactDTO contactDTO)
        {
            if (id != contactDTO.Id)
            {
                return BadRequest();
            }

            var contact = await _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            if (contact.Password != null && !User.HasClaim(ClaimTypes.Name, contact.Email))
            {
                return Unauthorized();
            }

            try
            {
                await _contactService.UpdateContact(contactDTO);

                return NoContent();
            }
            catch (DBConcurrencyException)
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _contactService.GetContact(id);
            if (contact == null)
            {
                return NotFound();
            }

            if (contact.Password != null && !User.HasClaim(ClaimTypes.Name, contact.Email))
            {
                return Unauthorized();
            }

            await _contactService.DeleteContact(id);

            return NoContent();
        }
    }
}