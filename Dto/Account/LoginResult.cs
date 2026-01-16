namespace News_App.Dto.Account
{
    /// <summary>
    /// Result of login operation
    /// </summary>
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public ResponseOfLogin? Response { get; set; }

        public static LoginResult Success(ResponseOfLogin response)
            => new() { Succeeded = true, Response = response };

        public static LoginResult Failure(string message)
            => new() { Succeeded = false, Message = message };
    }
}
