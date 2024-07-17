using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using api.Mappers;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(AuthService authService, ContactService contactService) : ControllerBase
    {
        private readonly AuthService _authService = authService;
        private readonly ContactService _contactService = contactService;

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthResponse>> Login(LoginDTO loginDTO)
        {
            try
            {
                var (token, user) = await _authService.Login(loginDTO);

                return new AuthResponse { Token = token, User = user };
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthResponse>> Register(RegisterDTO registerDTO)
        {
            try
            {
                var (token, user) = await _authService.Register(registerDTO);

                return CreatedAtAction(nameof(Login), new AuthResponse { Token = token, User = user });
            }
            catch (DBConcurrencyException)
            {
                return Conflict();
            }
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ContactDTO>> Me()
        {
            // return User;
            var email = User.Identity!.Name!;
            var contact = await _contactService.GetContact(email);
            return contact!.ToContactDto();
        }
    }
}