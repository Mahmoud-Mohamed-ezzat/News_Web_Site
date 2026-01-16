using System.ComponentModel.DataAnnotations;

namespace News_App.Dto.Account
{
    public class userDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
