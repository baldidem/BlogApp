using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entity
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
        public bool IsHome { get; set; }
        public DateTime UploadDate { get; set; }
        public string Description { get; set; }
        public List<BlogCategory>? BlogCategories { get; set; }
    }
}
