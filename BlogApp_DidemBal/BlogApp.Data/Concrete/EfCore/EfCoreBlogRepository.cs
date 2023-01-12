using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCoreBlogRepository : EfCoreGenericRepository<Blog>, IBlogRepository
    {
        public EfCoreBlogRepository(BlogAppContext _dbContext) : base(_dbContext)
        {

        }
        private BlogAppContext context
        {
            get
            {
                return _dbContext as BlogAppContext;
            }
        }

        public void CreateBlogWithCategories(Blog blog, int[] categoryIds)
        {
            context.Add(blog);
            context.SaveChanges();
            blog.BlogCategories = categoryIds
                .Select(catId => new BlogCategory
                {
                    BlogId = blog.Id,
                    CategoryId = catId
                }).ToList();
            context.SaveChanges();
        }
        public Blog GetBlogWithCategories(int id)
        {
            return context
                .Blogs
                .Where(b => b.Id == id)
                .Include(b => b.BlogCategories)
                .ThenInclude(b => b.Category)
                .FirstOrDefault();
        }

        public async Task<List<Blog>> GetHomePageBlogsAsync()
        {
            return await context
                .Blogs
                .Where(b=>b.IsHome == true)
                .ToListAsync();
        }
        public List<Blog> GetBlogsForCategory(int id)
        {
            var blogs = context
                .Blogs
                .Include(b => b.BlogCategories)
                .ThenInclude(b => b.Category)
                .Where(b => b.BlogCategories.Any(bc => bc.CategoryId == id))
                .ToList();

            return blogs;
        }

        public void UpdateBlogWithCats(Blog blog, int[] categoryIds)
        {
            var newBlog = context
    .Blogs
    .Include(b => b.BlogCategories)
    .FirstOrDefault(b => b.Id == blog.Id);

            newBlog.Title = blog.Title;
            newBlog.Summary = blog.Summary;
            newBlog.ImageUrl = blog.ImageUrl;
            newBlog.Author = blog.Author;
            newBlog.IsHome = blog.IsHome;
            newBlog.UploadDate = blog.UploadDate;
            newBlog.Description = blog.Description;
            newBlog.BlogCategories = categoryIds
                .Select(catId => new BlogCategory()
                {
                    BlogId = blog.Id,
                    CategoryId = catId
                }).ToList();
            context.Update(newBlog);
            context.SaveChanges();
        }

        public List<Blog> GetSearchResult(string search)
        {
            var result = context
                .Blogs
                .Where(b => b.Title.ToLower().Contains(search.ToLower()) || b.Summary.ToLower().Contains(search.ToLower()) || b.Description.ToLower().Contains(search.ToLower()) || b.Author.ToLower().Contains(search.ToLower()))
                .ToList();
            return result;
                
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            var blogs = await context
                .Blogs
                .ToListAsync();
            return blogs;
        }
    }
}
