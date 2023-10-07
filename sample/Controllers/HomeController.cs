using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Models;
using System.Diagnostics;

namespace sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
  

       //public ActionResult MaintenanceIndex()
       //     {
       //         var model = _context.AssetMaintenances.Include("Asset").ToList();
       //         return View(model);
       //     }

       //public ActionResult MovementIndex()
       //     {
       //         var model = _context.AssetMovements.Include("Asset").ToList();
       //         return View(model);
       //     }
        
    }
}