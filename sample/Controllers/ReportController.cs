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
            var assets = _context.Assets.ToList();
            ViewBag.Assets = assets;
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind("fromDate,toDate")] ReportViewModel reportViewModel)
        {
            var assets = _context.Assets
                .Where(
                    a =>
                        a.CreatedAt >= reportViewModel.FromDate
                        && a.CreatedAt <= reportViewModel.ToDate
                )
                .ToList();
            ViewBag.Assets = assets;
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
