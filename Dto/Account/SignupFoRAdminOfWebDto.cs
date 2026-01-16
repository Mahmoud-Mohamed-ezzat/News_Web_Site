using System.ComponentModel.DataAnnotations;

namespace News_App.Dto.Account
{
    public class SignupFoRAdminOfWebDto
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
