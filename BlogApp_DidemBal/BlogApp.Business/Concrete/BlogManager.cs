using BlogApp.Business.Abstract;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogManager(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public void Create(Blog blog)
        {
            throw new NotImplementedException();
        }

        public void CreateBlogWithCategories(Blog blog, int[] categoryIds)
        {
            _blogRepository.CreateBlogWithCategories(blog, categoryIds);
        }

        public void Delete(Blog blog)
        {
            _blogRepository.Delete(blog);
        }

        public List<Blog> GetAll()
        {
            return _blogRepository.GetAll();
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _blogRepository.GetAllAsync();
        }

        public List<Blog> GetBlogsForCategory(int id)
        {
            return _blogRepository.GetBlogsForCategory(id);
        }

        public Blog GetBlogWithCategories(int id)
        {
            return _blogRepository.GetBlogWithCategories(id);
        }

        public Blog GetById(int id)
        {
            return _blogRepository.GetById(id);
        }

        public async Task<List<Blog>> GetHomePageBlogsAsync()
        {
            return await _blogRepository.GetHomePageBlogsAsync();
        }

        public List<Blog> GetSearchResult(string search)
        {
            return _blogRepository.GetSearchResult(search);
        }

        public void Update(Blog blog)
        {
            _blogRepository.Update(blog);
        }

        public void UpdateBlogWithCats(Blog blog, int[] categoryIds)
        {
            _blogRepository.UpdateBlogWithCats(blog, categoryIds);
        }
    }
}
