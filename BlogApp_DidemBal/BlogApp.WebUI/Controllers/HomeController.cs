using BlogApp.Business.Abstract;
using BlogApp.Entity;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogService _blogService;
        private ICategoryService _categoryService;

        public HomeController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {

            List<Blog> blogs = await _blogService.GetHomePageBlogsAsync();
            ViewBag.Categories = _categoryService.GetAll();

            return View(blogs);
        }
        public async Task<IActionResult> BlogList()
        {
            List<Blog> blogs = await _blogService.GetAllAsync();
            ViewBag.Categories = _categoryService.GetAll();
            return View(blogs);
        }
        public IActionResult BlogDetails(int id)
        {
            Blog blog = _blogService.GetById(id);
            ViewBag.Categories = _categoryService.GetAll();
            return View(blog);

        }
        public IActionResult BlogsForCategory(int id)
        {
            var blogs = _blogService.GetBlogsForCategory(id);
            ViewBag.Categories = _categoryService.GetAll();
            return View(blogs);
        }
        public IActionResult Search(string search)
        {
            var result = _blogService.GetSearchResult(search);
            ViewBag.Categories = _categoryService.GetAll();
            return View(result);
        }


    }
}