using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab1.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers
{
    public class HomeController : Controller
    {
        private PhotostudioInnoDbContext db;

        public HomeController(PhotostudioInnoDbContext context)
        {
            db = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Catalog()
        {

            List<Hall> halls = db.Halls.Include(h => h.HallEquipments).ToList();
            return View(halls);
        }

    }
}