using AmazingTeamTaskManager.Core.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AmazingTeamTaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string login, string email, string password, string firstName, string lastName)
        {
            try
            {
                var token = await _authService.RegisterAsync(login, email, password, firstName, lastName);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            try
            {
                var token = await _authService.LoginAsync(login, password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
