using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Models;

namespace sample.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null && toDate != null)
            {
                var assets = _context.Assets
                    .Where(a => a.PurchaseDate >= fromDate && a.PurchaseDate <= toDate)
                    .ToList();

                return View("Report", assets);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Report(DateTime fromDate, DateTime toDate)
        {
            var assets = _context.Assets
                .Where(a => a.PurchaseDate >= fromDate && a.PurchaseDate <= toDate)
                .ToList();

            return View("Report", assets);
        }
    }
}
