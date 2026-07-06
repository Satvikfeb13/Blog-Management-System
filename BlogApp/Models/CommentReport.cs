using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class CommentReport
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CommentId { get; set; }
        
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }

        public string? ReportedByUserId { get; set; }
        
        [ForeignKey("ReportedByUserId")]
        public ApplicationUser? ReportedByUser { get; set; }

        public Enums.ReportReason Reason { get; set; }
        
        public string? AdditionalDetails { get; set; }
        
        public DateTime ReportDate { get; set; } = DateTime.Now;
        
        public bool IsResolved { get; set; } = false;
    }
}
