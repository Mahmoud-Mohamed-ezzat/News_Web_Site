namespace News_App.Dto.Account
{
    /// <summary>
    /// Result of account operations (signup, email confirmation, logout, etc.)
    /// </summary>
    public class AccountOperationResult
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string? ConfirmationToken { get; set; }

        public static AccountOperationResult Success(string? message = null, string? confirmationToken = null)
            => new() { Succeeded = true, Message = message, ConfirmationToken = confirmationToken };

        public static AccountOperationResult Failure(string message, IEnumerable<string>? errors = null)
            => new() { Succeeded = false, Message = message, Errors = errors };
    }
}
