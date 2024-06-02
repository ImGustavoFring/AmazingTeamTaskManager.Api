using AmazingTeamTaskManager.Core.Models.UserModel;
using AmazingTeamTaskManager.Core.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazingTeamTaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManagementService _userManagementService;

        public UserManagementController(UserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userManagementService.GetOneByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userManagementService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/change-login")]
        public async Task<IActionResult> ChangeLogin(Guid id, [FromBody] string newLogin)
        {
            try
            {
                await _userManagementService.ChangeLoginAsync(id, newLogin);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] string newPassword)
        {
            try
            {
                await _userManagementService.ChangePasswordAsync(id, newPassword);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/change-email")]
        public async Task<IActionResult> ChangeEmail(Guid id, [FromBody] string newEmail)
        {
            try
            {
                await _userManagementService.ChangeEmailAsync(id, newEmail);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{userId}/change-first-name")]
        public async Task<IActionResult> ChangeFirstName(Guid userId, [FromBody] string newFirstName)
        {
            try
            {
                await _userManagementService.ChangeFirstNameAsync(userId, newFirstName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{userId}/change-last-name")]
        public async Task<IActionResult> ChangeLastName(Guid userId, [FromBody] string newLastName)
        {
            try
            {
                await _userManagementService.ChangeLastNameAsync(userId, newLastName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{userId}/change-description")]
        public async Task<IActionResult> ChangeDescription(Guid userId, [FromBody] string newDescription)
        {
            try
            {
                await _userManagementService.ChangeDescriptionAsync(userId, newDescription);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/change-role")]
        public async Task<IActionResult> ChangeUserRole(Guid id, [FromBody] RoleInSystem newRole)
        {
            try
            {
                await _userManagementService.ChangeUserRoleAsync(id, newRole);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userManagementService.DeleteOneAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
