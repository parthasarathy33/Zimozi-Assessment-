using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var (success, token) = await _authService.LoginAsync(model.Email, model.Password);
            if (!success)
                return Unauthorized(new { message = token });

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var (success, message) = await _authService.RegisterAsync(
                model.Email,
                model.Password,
                model.FirstName,
                model.LastName);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var result = await _authService.AssignRoleAsync(model.UserId, model.Role);
            if (!result)
                return BadRequest(new { message = "Failed to assign role" });

            return Ok(new { message = "Role assigned successfully" });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
} 
} 