using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using News_App.Dto.Account;
using News_App.Interfaces;
using News_App.Mapper.Account;
using News_App.Models;

namespace News_App.services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenServices _tokenServices;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TokenServices tokenServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
        }

        public async Task<AccountOperationResult> CreateUserWithRoleAsync(SignupFoRAdminOfWebDto model, string roleName)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                return AccountOperationResult.Failure(
                    "User creation failed",
                    createResult.Errors.Select(e => e.Description));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
            {
                // Rollback: delete user if role assignment fails
                await _userManager.DeleteAsync(user);
                return AccountOperationResult.Failure(
                    "Role assignment failed",
                    roleResult.Errors.Select(e => e.Description));
            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return AccountOperationResult.Success("Registration successful", confirmationToken);
        }

        public async Task<AccountOperationResult> ConfirmEmailAsync(string email, string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
                return AccountOperationResult.Failure("Invalid email or confirmation code");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return AccountOperationResult.Failure("Invalid email or confirmation code");

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return AccountOperationResult.Failure(
                    "Email confirmation failed",
                    result.Errors.Select(e => e.Description));
            }

            return AccountOperationResult.Success("Email verified successfully");
        }

        public async Task<LoginResult> LoginAsync(LoginDto login)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
                return LoginResult.Failure("Invalid email or password");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
                return LoginResult.Failure("Invalid email or password");

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                return LoginResult.Failure("Email not verified. Please verify your email first.");

            var result = await _tokenServices.GenerateToken(user);
            _tokenServices.SetRefreshTokeninCookies(result.RefreshToken!, result.RefreshTokenExpiration);

            return LoginResult.Success(result);
        }

        public async Task<AccountOperationResult> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return AccountOperationResult.Failure("User not found");

            await _tokenServices.RevokeRefreshToken();
            await _signInManager.SignOutAsync();
            await _userManager.UpdateSecurityStampAsync(user);

            return AccountOperationResult.Success("Logged out successfully");
        }

        public async Task<List<GetPublisherDto>> GetPublishersAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Publisher");
            var userIds = usersInRole.Select(u => u.Id).ToHashSet();

            var publishers = await _userManager.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Include(u => u.NewsPagesOfPublisher)
                .ToListAsync();

            return publishers.Select(p => p.ToGetPublishersDto()).ToList();
        }
    }
}
