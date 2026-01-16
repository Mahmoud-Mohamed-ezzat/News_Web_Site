using News_App.Dto.Account;

namespace News_App.Interfaces
{
    /// <summary>
    /// Interface for account-related operations
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates a new user and assigns a role
        /// </summary>
        Task<AccountOperationResult> CreateUserWithRoleAsync(SignupFoRAdminOfWebDto model, string roleName);

        /// <summary>
        /// Confirms user's email with the provided code
        /// </summary>
        Task<AccountOperationResult> ConfirmEmailAsync(string email, string code);

        /// <summary>
        /// Authenticates user and returns tokens
        /// </summary>
        Task<LoginResult> LoginAsync(LoginDto login);

        /// <summary>
        /// Logs out user and revokes refresh token
        /// </summary>
        Task<AccountOperationResult> LogoutAsync(string userId);

        /// <summary>
        /// Gets all publishers with their news pages
        /// </summary>
        Task<List<GetPublisherDto>> GetPublishersAsync();
    }
}
