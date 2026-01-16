using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News_App.services;

namespace News_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenServices _tokenServices;

        public TokenController(TokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        /// <summary>
        /// Get a new JWT token using refresh token from cookies (JWT only, no new refresh token)
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh-jwt")]
        public async Task<IActionResult> ReturnNewJwtTokenByRefreshTokenFromCookie()
        {
            var (success, token, message) = await _tokenServices.GetNewJwtFromRefreshTokenCookie();

            if (!success)
                return Unauthorized(new { Message = message });

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Refresh both JWT and refresh token using refresh token from cookies
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _tokenServices.RefreshTokenAsync();

            if (!result.IsAuthenticated)
                return Unauthorized(new { Message = result.Message });

            // Set the new refresh token in cookies
            _tokenServices.SetRefreshTokeninCookies(result.RefreshToken!, result.RefreshTokenExpiration);

            return Ok(result);
        }

        /// <summary>
        /// Refresh both JWT and refresh token using refresh token from request body
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh-with-token")]
        public async Task<IActionResult> RefreshWithToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request?.RefreshToken))
                return BadRequest(new { Message = "Refresh token is required" });

            var result = await _tokenServices.RefreshTokenAsync(request.RefreshToken);

            if (!result.IsAuthenticated)
                return Unauthorized(new { Message = result.Message });

            // Set the new refresh token in cookies
            _tokenServices.SetRefreshTokeninCookies(result.RefreshToken!, result.RefreshTokenExpiration);

            return Ok(result);
        }
    }


    public class RefreshTokenRequest
    {
        public string? RefreshToken { get; set; }
    }
}
