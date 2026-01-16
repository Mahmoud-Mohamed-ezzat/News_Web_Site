using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using News_App.Controllers;


namespace News_App.Models
{
    public class User : IdentityUser
    {
        public List<Newspage>? NewsPagesOfPublisher { get; set; } //for news Pages of puplisher
        public List<Post>? posts { get; set; } //for post of puplisher 
        public Newspage? Newspages { get; set; } // for admin of page
        public ICollection<RefreshToken>? RefreshTokens { get; set; } //for refresh tokens
    }
}
