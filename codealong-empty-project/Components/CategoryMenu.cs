using codealong_pie_project.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace codealong_pie_project.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryMenu(ICategoryRepository categories)
        {
            categoryRepository = categories;
        }

        public IViewComponentResult Invoke()
        {
            var categories = categoryRepository.AllCategories.OrderBy(c => c.CategoryName);
            return View(categories);
        }
    }
}
