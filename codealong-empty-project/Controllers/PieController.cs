using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using codealong_pie_project.Models;
using codealong_pie_project.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace codealong_pie_project.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository pieRepository;
        private readonly ICategoryRepository categoryRepository;

        public PieController(IPieRepository pieRep, ICategoryRepository categoryRep)
        {
            pieRepository = pieRep;
            categoryRepository = categoryRep;

        }
        //public ViewResult List() //viewresult - inbyggd typ för view
        //{
        //    PieListViewModel pieListViewModel = new PieListViewModel(); //istället för viewbag, vanlig klass med properties
        //    pieListViewModel.Pies = pieRepository.AllPies;
        //    pieListViewModel.StoreName = "Jossans pajer!";
        //    return View(pieListViewModel); //skickar med hela objektet

        //    //ViewBag.StoreName = "Jossans pajer!";
        //    //return View(pieRepository.AllPies); //Kan bara skicka med en parameter/data

        //}
        //byter ut list-metod till att kunna ta parameter category
        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string currentCategory;

            //ingen spec cat vald
            if (string.IsNullOrEmpty(category))
            {
                pies = pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                pies = pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.PieId);
                currentCategory = categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            return View(new PieListViewModel
            {
                Pies = pies,
                CurrentCategory = currentCategory
            });
        }
        public  IActionResult Details(int id)
        {
            var pie = pieRepository.GetPieById(id); //hämtar paj med rätt id med pierepositorys metod (firstordefault från db)
            if (pie == null)//om pajen inte kan hämtas blir värdet null
                return NotFound(); //returnerar 404 sida
            return View(pie);
        }
        
    }
}
