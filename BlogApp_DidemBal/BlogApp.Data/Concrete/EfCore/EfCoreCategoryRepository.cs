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
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EfCoreCategoryRepository(BlogAppContext _dbContext) : base(_dbContext)
        {
        }
        private BlogAppContext context
        {
            get
            {
                return _dbContext as BlogAppContext;
            }
        }
    }
}
