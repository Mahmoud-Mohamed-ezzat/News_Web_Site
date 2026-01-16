using Microsoft.EntityFrameworkCore;
using System; // Import System namespace for DateTime and Guid types
using System.ComponentModel.DataAnnotations; // Import DataAnnotations for validation attributes
using System.ComponentModel.DataAnnotations.Schema; // Import Schema for database table/column configuration

namespace News_App.Models // Define the namespace matching other models in the project
{
    public class RefreshToken
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public string UserId { get; set; }
        public User User { get; set; }
    }
}

