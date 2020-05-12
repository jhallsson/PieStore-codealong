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
    public class HomeController : Controller
    {
        private readonly IPieRepository pieRepository;
        public HomeController(IPieRepository pieRep)
        {
            pieRepository = pieRep;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                //hämtar Pies i piesoftheweek från Pierepository och lägger in i viewmodellens property
                PiesOfTheWeek = pieRepository.PiesOfTheWeek

            };
            //skickar med en homeviewmodel till vyn
            return View(homeViewModel);
        }
    }
}
