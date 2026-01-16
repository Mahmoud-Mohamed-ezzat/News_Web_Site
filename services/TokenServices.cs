using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using News_App.Dto.Account;
using News_App.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace News_App.services
{
    public class TokenServices
    {
        readonly IConfiguration _config;
        readonly UserManager<User> _user;
        readonly SymmetricSecurityKey _key;
        readonly IHttpContextAccessor _httpContextAccessor;
        public TokenServices(UserManager<User> user, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _user = user;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> createToken(User user)
        {
            var Role = (await _user.GetRolesAsync(user)).FirstOrDefault();
            var claims = new List<Claim>{
           new Claim(JwtRegisteredClaimNames.Sub,user.Id),
           new Claim(JwtRegisteredClaimNames.GivenName,user.UserName),
           new Claim(JwtRegisteredClaimNames.Email,user.Email),
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
           new Claim(ClaimTypes.Role,Role),
          };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Audience = _config["JWT:Audience"],
                Issuer = _config["JWT:Issuer"],
                Expires = DateTime.Now.AddMinutes(10)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<RefreshToken> GenerateRefreshToken(User user)
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddMonths(2),
                CreatedOn = DateTime.UtcNow,
                UserId = user.Id
            };
        }
        public async Task<ResponseOfLogin> GenerateToken(User user)
        {
            var role = (await _user.GetRolesAsync(user)).FirstOrDefault();
            var token = await createToken(user);
            var responseOfLogin = new ResponseOfLogin
            {
                Username = user.UserName,
                IsAuthenticated = true,
                Id = user.Id,
                Email = user.Email,
                Role = role,
                Token = token
            };

            if (user.RefreshTokens != null && user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                responseOfLogin.RefreshToken = activeRefreshToken!.Token;
                responseOfLogin.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = await GenerateRefreshToken(user);
                if (user.RefreshTokens == null)
                    user.RefreshTokens = new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                await _user.UpdateAsync(user);
                responseOfLogin.RefreshToken = refreshToken.Token;
                responseOfLogin.RefreshTokenExpiration = refreshToken.ExpiresOn;
            }
            return responseOfLogin;
        }
        public async Task<bool> RevokeRefreshToken()
        {
            var Token = getRefreshTokenFromCookies();
            if (Token == null)
                return false;

            var user = await _user.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == Token));
            if (user == null)
                return false;
            var refreshToken = user.RefreshTokens!.Single(t => t.Token == Token);
            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _user.UpdateAsync(user);
            return true;
        }
        public void SetRefreshTokeninCookies(string token, DateTime expires)
        {

            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Expires = expires.ToLocalTime(),
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOption);
        }
        public string? getRefreshTokenFromCookies()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            return refreshToken;
        }

        /// <summary>
        /// Get refresh token from cookies, find user, and return new JWT only
        /// </summary>
        public async Task<(bool Success, string? Token, string? Message)> GetNewJwtFromRefreshTokenCookie()
        {
            // Step 1: Get refresh token from cookies
            var refreshToken = getRefreshTokenFromCookies();
            if (string.IsNullOrEmpty(refreshToken))
                return (false, null, "Refresh token not found in cookies");

            // Step 2: Find user who has this refresh token
            var user = await _user.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == refreshToken));

            if (user == null)
                return (false, null, "Invalid refresh token");

            // Step 3: Check if refresh token is still active
            var storedRefreshToken = user.RefreshTokens!.Single(t => t.Token == refreshToken);
            if (!storedRefreshToken.IsActive)
                return (false, null, "Refresh token is expired or revoked");

            // Step 4: Generate new JWT for this user
            var newJwt = await createToken(user);

            return (true, newJwt, null);
        }
        public async Task<ResponseOfLogin> RefreshTokenAsync()
        {
            var refreshTokenFromCookies = getRefreshTokenFromCookies();
            return await RefreshTokenAsync(refreshTokenFromCookies);
        }

        public async Task<ResponseOfLogin> RefreshTokenAsync(string? refreshToken)
        {

            if (string.IsNullOrEmpty(refreshToken))
            {
                return new ResponseOfLogin { IsAuthenticated = false, Message = "Refresh token is required" };
            }

            var user = await _user.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == refreshToken));

            if (user == null)
            {
                return new ResponseOfLogin { IsAuthenticated = false, Message = "Invalid refresh token" };
            }

            var storedRefreshToken = user.RefreshTokens!.Single(t => t.Token == refreshToken);

            if (!storedRefreshToken.IsActive)
            {
                return new ResponseOfLogin { IsAuthenticated = false, Message = "Refresh token is expired or revoked" };
            }

            // Revoke the old refresh token
            storedRefreshToken.RevokedOn = DateTime.UtcNow;

            // Generate new refresh token
            var newRefreshToken = await GenerateRefreshToken(user);
            user.RefreshTokens!.Add(newRefreshToken);
            await _user.UpdateAsync(user);

            // Generate new JWT token
            var jwtToken = await createToken(user);
            var role = (await _user.GetRolesAsync(user)).FirstOrDefault();

            return new ResponseOfLogin
            {
                IsAuthenticated = true,
                Username = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Role = role,
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }
    }
}
