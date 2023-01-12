using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Abstract
{
    public interface IBlogService
    {
        #region Generic
        List<Blog> GetAll();
        Task<List<Blog>> GetAllAsync();
        Blog GetById(int id);
        void Create(Blog blog);
        void Update(Blog blog);
        void Delete(Blog blog);
        #endregion


        #region Spesific

        Task<List<Blog>> GetHomePageBlogsAsync();
        Blog GetBlogWithCategories(int id);
        void UpdateBlogWithCats(Blog blog, int[] categoryIds);
        void CreateBlogWithCategories(Blog blog, int[] categoryIds);
        List<Blog> GetBlogsForCategory(int id);
        List<Blog> GetSearchResult(string search);
        #endregion
    }
}
