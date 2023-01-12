using BlogApp.Business.Abstract;
using BlogApp.Core;
using BlogApp.Entity;
using BlogApp.WebUI.Identity;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace BlogApp.WebUI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(IBlogService blogService, ICategoryService categoryService, SignInManager<MyIdentityUser> signInManager, RoleManager<IdentityRole> roleManager, UserManager<MyIdentityUser> userManager)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #region Blog

        public IActionResult BlogEdit(int id)
        {
            Blog blog = _blogService.GetBlogWithCategories(id);
            BlogEditModel blogEditModel = new BlogEditModel()
            {
                Id = blog.Id,
                Title = blog.Title,
                Summary = blog.Summary,
                ImageUrl = blog.ImageUrl,
                Author = blog.Author,
                IsHome = blog.IsHome,
                UploadDate = blog.UploadDate,
                Description = blog.Description,
                SelectedCategories = blog.BlogCategories.Select(bc => bc.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(blogEditModel);
        }

        [HttpPost]
        public IActionResult BlogEdit(BlogEditModel blogEditModel, IFormFile file, int[] categoryIds)
        {
            string imageUrl = "";
            if (ModelState.IsValid && categoryIds.Length > 0)
            {
                Blog blog = _blogService.GetById(blogEditModel.Id);
                if (file == null)
                {
                    imageUrl = blog.ImageUrl;
                }
                else
                {
                    imageUrl = file.FileName;
                }
                if (blog == null)
                {
                    return NotFound();
                }
                blog.Title = blogEditModel.Title;
                blog.Summary = blogEditModel.Summary;
                blog.ImageUrl = imageUrl;
                blog.Author = blogEditModel.Author;
                blog.IsHome = blogEditModel.IsHome;
                blog.UploadDate = DateTime.Now;
                blog.Description = blogEditModel.Description;

                _blogService.UpdateBlogWithCats(blog, categoryIds);
                return RedirectToAction("Index", "Home");
            }

            if (categoryIds.Length == 0)
            {
                ViewBag.CategoryErrorMessage = "Choose a category!";
                ViewBag.Categories = _categoryService.GetAll();
                if (file == null)
                {
                    Blog blog = _blogService.GetById(blogEditModel.Id);
                    imageUrl = blog.ImageUrl;
                }
                blogEditModel.ImageUrl = imageUrl;
            }

            return View(blogEditModel);
        }

        public IActionResult BlogCreate()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult BlogCreate(BlogEditModel editModel, IFormFile file, int[] categoryIds)
        {
            string imageUrl = "";
            editModel.UploadDate = DateTime.Now;
            if (ModelState.IsValid && categoryIds.Length > 0 && file != null)
            {
                Blog blog = new Blog()
                {
                    Title = editModel.Title,
                    Summary = editModel.Summary,
                    Description = editModel.Description,
                    IsHome = editModel.IsHome,
                    UploadDate = DateTime.Now,
                    ImageUrl = file.FileName,
                    Author = editModel.Author
                };

                _blogService.CreateBlogWithCategories(blog, categoryIds);
                return RedirectToAction("Index", "Home");
            }

            if (file == null && categoryIds.Length > 0)
            {
                ViewBag.ImageErrorMessage = "Choose an image!";
                ViewBag.Categories = _categoryService.GetAll();
            }

            if (categoryIds.Length == 0)
            {
                if (file == null)
                {
                    ViewBag.ImageErrorMessage = "Choose an image!";
                }
                if (file != null)
                {
                    imageUrl = file.FileName;
                    editModel.ImageUrl = imageUrl;
                }
                ViewBag.CategoryErrorMessage = "Choose a category!";
                ViewBag.Categories = _categoryService.GetAll();
            }
            else
            {
                if (file != null)
                {
                    imageUrl = file.FileName;
                    editModel.ImageUrl = imageUrl;
                }
                ViewData["SelectedCategories"] = categoryIds;
            }
            return View(editModel);
        }
        public IActionResult BlogDelete(int id)
        {
            Blog blog = _blogService.GetById(id);
            _blogService.Delete(blog);
            return RedirectToAction("Index", "Home");
        }


        #endregion
        #region Category
        public IActionResult CategoryCreate(string categoryAdded)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    CategoryName = categoryAdded
                };
                _categoryService.Create(category);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult CategoryDelete(int id)
        {
            Category category = _categoryService.GetById(id);
            _categoryService.Delete(category);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult CategoryEdit(int id)
        {
            var category = _categoryService.GetById(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                var cat = _categoryService.GetById(category.Id);
                cat.CategoryName = category.CategoryName;
                _categoryService.Update(cat);
                return Redirect("~/");
            }
            return View(category);
        }
        #endregion
        #region Role
        public async Task<IActionResult> RoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole() { Name = roleModel.Name };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = Message.CreateMessage("Notification", "Role is created successfully", "success");
                    return RedirectToAction("RoleList");
                }
            }
            return View(roleModel);


        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var users = await _userManager.Users.ToListAsync();
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<MyIdentityUser>();
            var nonMembers = new List<MyIdentityUser>();
            foreach (var user in users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            RoleDetails roleDetails = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(roleDetails);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel roleEditModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in roleEditModel.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in roleEditModel.IdsToRemove ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

            }
            return Redirect($"/Admin/RoleEdit/{roleEditModel.RoleId}");
        }
        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) { return NotFound(); }
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    TempData["Message"] = Message.CreateMessage("Unsuccessful!", "There are users in this role, you need to delete the users first.", "danger");
                    return RedirectToAction("RoleList");
                }
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Message"] = Message.CreateMessage("Success!", "You deleted the role", "danger");
            }

            return RedirectToAction("RoleList");

        }
        #endregion
        #region User
        public async Task<IActionResult> UserList()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        public async Task<IActionResult> UserCreate()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserCreate(UserModel userModel, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = new MyIdentityUser()
                {
                    UserName = userModel.UserName,
                    Name = userModel.Name,
                    Surname = userModel.Surname,
                    Email = userModel.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                    TempData["Message"] = Message.CreateMessage("Notification!", "You created a new user successfully", "success");
                    return RedirectToAction("UserList");
                }
            }
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.SelectedRoles = selectedRoles;
            return View(userModel);
        }
        public async Task<IActionResult> UserEdit(string id)

        {
            var user = await _userManager.FindByIdAsync(id);
            var userModel = new UserModel()
            {
                UserId = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                SelectedRoles = await _userManager.GetRolesAsync(user)
            };

            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserModel userModel, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userModel.UserId);
                if (user != null)
                {
                    user.Name = userModel.Name;
                    user.Surname = userModel.Surname;
                    user.UserName = userModel.UserName;
                    user.Email = userModel.Email;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        userModel.SelectedRoles = userModel.SelectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, userModel.SelectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(userModel.SelectedRoles).ToArray<string>());
                        TempData["Message"] = Message.CreateMessage("Notification", "Registration successfully edited.", "success");
                        return RedirectToAction("UserList");
                    }
                    ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                    return View(userModel);
                }
                TempData["Message"] = Message.CreateMessage("Warning", "User not found!", "danger");
            }
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(userModel);
        }
        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = Message.CreateMessage("Notification!", "You deleted user successfully", "danger");
                }
            }
            return RedirectToAction("UserList");

        }
        public async Task<IActionResult> PasswordEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                PasswordModel passwordModel = new PasswordModel()
                {
                    UserId = user.Id
                };
                return View(passwordModel);
            }
            else
            {
                return RedirectToAction("UserList");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PasswordEdit(PasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(passwordModel.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordModel.Password);
                var result = await _userManager.UpdateAsync(user);
                TempData["Message"] = Message.CreateMessage("Notification", "Password has changed successfully", "success");
                return RedirectToAction("UserList");
            }
            if (passwordModel.Password != passwordModel.RePassword)
            {
                ViewBag.PasswordErrorMessage = "Passwords don't match!";
            }
            return View(passwordModel);
        }
        #endregion

    }
}


