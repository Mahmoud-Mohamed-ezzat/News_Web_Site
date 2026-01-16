using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_App.Dto.Account;
using News_App.Interfaces;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailConfirmationDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.ConfirmEmailAsync(request.Email, request.Code);

            if (!result.Succeeded)
                return BadRequest(new { result.Message, result.Errors });

            return Ok(new { result.Message });
        }

        [AllowAnonymous]
        [HttpPost("signup/admin")]
        public async Task<IActionResult> SignupAdmin([FromBody] SignupFoRAdminOfWebDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await CreateUserWithRoleResponse(model, "Admin");
        }

        [AllowAnonymous]
        [HttpPost("signup/user")]
        public async Task<IActionResult> SignupUser([FromBody] SignupFoRAdminOfWebDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await CreateUserWithRoleResponse(model, "User");
        }

        [AllowAnonymous]
        [HttpPost("signup/publisher")]
        public async Task<IActionResult> SignupPublisher([FromBody] SignupFoRAdminOfWebDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await CreateUserWithRoleResponse(model, "Publisher");
        }

        [AllowAnonymous]
        [HttpPost("signup/page-admin")]
        public async Task<IActionResult> SignupPageAdmin([FromBody] SignupFoRAdminOfWebDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await CreateUserWithRoleResponse(model, "AdminOfPage");
        }

        /// <summary>
        /// Helper to convert service result to HTTP response
        /// </summary>
        private async Task<IActionResult> CreateUserWithRoleResponse(SignupFoRAdminOfWebDto model, string roleName)
        {
            var result = await _accountService.CreateUserWithRoleAsync(model, roleName);

            if (!result.Succeeded)
                return BadRequest(new { result.Message, result.Errors });

            return Ok(new { result.Message, result.ConfirmationToken });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.LoginAsync(login);

            if (!result.Succeeded)
                return Unauthorized(new { result.Message });

            return Ok(result.Response);
        }

        [Authorize]
        [HttpGet("publishers")]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _accountService.GetPublishersAsync();
            return Ok(publishers);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "User not found" });

            var result = await _accountService.LogoutAsync(userId);

            if (!result.Succeeded)
                return BadRequest(new { result.Message });

            // Clear the refresh token cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { result.Message });
        }
    }
}
