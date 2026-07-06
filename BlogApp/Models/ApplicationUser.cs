using Microsoft.AspNetCore.Identity;

namespace BlogApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        
        // Navigation properties
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}
