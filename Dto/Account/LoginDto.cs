using System.ComponentModel.DataAnnotations;

namespace News_App.Dto.Account
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
