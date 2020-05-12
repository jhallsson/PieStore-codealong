using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Models
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext appDbContext;
        public CategoryRepository(AppDbContext appDb)
        {
            appDbContext = appDb;
        }
        public IEnumerable<Category> AllCategories =>appDbContext.Categories; //dbset prop
    }
}
