using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Title is required")]
        [MaxLength(400,ErrorMessage ="Title cannot exceed 400 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Author is required")]
        [MaxLength(100, ErrorMessage = "name cannot exceed 100 characters")] 
        public string Author { get; set; }
        public string? CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public ApplicationUser? CreatedByUser { get; set; }
        
        public BlogApp.Models.Enums.PostStatus Status { get; set; } = BlogApp.Models.Enums.PostStatus.Draft;
        
        public string? ApprovedByAdminId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectedReason { get; set; }
        public DateTime? LastModified { get; set; }

        [ValidateNever]
        public string FeatureImagePath { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }=DateTime.Now;

        public string? Slug { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int ReadTime { get; set; } // Estimated read time in minutes

        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [ValidateNever]
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        [ValidateNever]
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        [ValidateNever]
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

    }
}
