using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Category name is required")]
        [MaxLength(100,ErrorMessage ="Category name can not exceed 100 char")]
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Post> Posts { get; set; }




    }
}
 