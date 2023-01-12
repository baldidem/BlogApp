using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogAppContext : DbContext
    {
        public BlogAppContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BlogCategory>()
                .HasKey(ba => new
                {
                    ba.BlogId,
                    ba.CategoryId
                });
            modelBuilder
                .Entity<Category>()
                .HasData(
                new Category() { Id = 1, CategoryName = "Life" },
                new Category() { Id = 2, CategoryName = "Music" },
                new Category() { Id = 3, CategoryName = "Sports" },
                new Category() { Id= 4, CategoryName = "Psychology"}
                );

            modelBuilder
                .Entity<Blog>()
                .HasData(
                new Blog() { Id = 1, Title = "Life is good", Summary = "Life is so good. I'm so glad that i love life", ImageUrl = "1.jpg", Author = "Didem", IsHome = true, UploadDate = DateTime.Now.AddDays(-10), Description= "Life is so good. I'm so glad that i love life description" },
                new Blog() { Id = 2, Title = "Music is everything", Summary = "Music is everything for any kind of living being.", ImageUrl = "2.jpg", Author = "John", IsHome = true, UploadDate = DateTime.Now.AddDays(-8), Description = "Music is everything for any kind of living being description." },
                new Blog() { Id = 3, Title = "Sport is crucial", Summary = "We need to do sports for living a long and healthy life", ImageUrl = "3.jpg", Author = "Emily", IsHome = false, UploadDate = DateTime.Now.AddDays(-6), Description = "We need to do sports for living a long and healthy life description" },
                new Blog() { Id = 4, Title = "Living life", Summary = "Living life is a kind of art even if we don't realize", ImageUrl = "4.jpg", Author = "Marcus", IsHome = true, UploadDate = DateTime.Now.AddDays(-4), Description= "Living life is a kind of art even if we don't realize description" },
                new Blog() { Id = 5, Title = "Being Positive", Summary = "We need to stay away from toxic positivity.", ImageUrl = "5.jpg", Author = "Angelica", IsHome = false, UploadDate = DateTime.Now.AddDays(-2), Description = "We need to stay away from toxic positivity description" }
                );

            modelBuilder
                .Entity<BlogCategory>()
                .HasData(
                new BlogCategory() { BlogId = 1, CategoryId = 1 },
                new BlogCategory() { BlogId = 2, CategoryId = 2 },
                new BlogCategory() { BlogId = 3, CategoryId = 3 },
                new BlogCategory() { BlogId = 4, CategoryId = 1 },
                new BlogCategory() { BlogId = 5, CategoryId = 4 }
                );
        }
    }
}
