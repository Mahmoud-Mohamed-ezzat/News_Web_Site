using System.ComponentModel.DataAnnotations;

namespace News_App.Dto.Account
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
