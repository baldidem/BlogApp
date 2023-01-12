using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Abstract
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<List<Blog>> GetHomePageBlogsAsync();
        Blog GetBlogWithCategories(int id);
        void UpdateBlogWithCats(Blog blog, int[] categoryIds);
        void CreateBlogWithCategories(Blog blog, int[] categoryIds);
        List<Blog> GetBlogsForCategory(int id);
        List<Blog> GetSearchResult(string search);
        Task<List<Blog>> GetAllAsync();





    }
}
