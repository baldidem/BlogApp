using BlogApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.WebUI.Models
{
    public class BlogEditModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Summary is required!")]
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Author is required!")]
        public string Author { get; set; }
        public bool IsHome { get; set; }
        public DateTime? UploadDate { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        public List<Category> SelectedCategories { get; set; } =null;

    }
}
