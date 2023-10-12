using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IActionResult Index(
            [Bind("FromDate,ToDate,Location,Status,Category,Type")] ReportViewModel reportViewModel
        )
        {
            var assets = new List<Asset>();

            if (reportViewModel.Type == "Assets")
            {
                assets = _context.Assets
                    .Where(
                        a =>
                            (
                                !reportViewModel.FromDate.HasValue
                                || a.CreatedAt >= reportViewModel.FromDate
                            )
                            && (
                                !reportViewModel.ToDate.HasValue
                                || a.CreatedAt <= reportViewModel.ToDate
                            )
                            && (
                                reportViewModel.Location == null
                                || a.Location == reportViewModel.Location
                            )
                            && (
                                reportViewModel.Status == null || a.status == reportViewModel.Status
                            )
                            && (
                                reportViewModel.Category == null
                                || a.Category == reportViewModel.Category
                            )
                    )
                    .ToList();
            }
            else if (reportViewModel.Type == "Maintainance")
            {
                assets = _context.AssetMaintenances
                    .Where(
                        a =>
                            (
                                !reportViewModel.FromDate.HasValue
                                || a.Asset!.CreatedAt >= reportViewModel.FromDate
                            )
                            && (
                                !reportViewModel.ToDate.HasValue
                                || a.Asset!.CreatedAt <= reportViewModel.ToDate
                            )
                            && (
                                reportViewModel.Location == null
                                || a.Asset!.Location == reportViewModel.Location
                            )
                            && (
                                reportViewModel.Status == null
                                || a.Asset!.status == reportViewModel.Status
                            )
                            && (
                                reportViewModel.Category == null
                                || a.Asset!.Category == reportViewModel.Category
                            )
                    )
                    .Select(a => a.Asset!)
                    .ToList();
            }
            else if (reportViewModel.Type == "Movement")
            {
                assets = _context.AssetMovements
                    .Where(
                        a =>
                            (
                                !reportViewModel.FromDate.HasValue
                                || a.Asset!.CreatedAt >= reportViewModel.FromDate
                            )
                            && (
                                !reportViewModel.ToDate.HasValue
                                || a.Asset!.CreatedAt <= reportViewModel.ToDate
                            )
                            && (
                                reportViewModel.Location == null
                                || a.Asset!.Location == reportViewModel.Location
                            )
                            && (
                                reportViewModel.Status == null
                                || a.Asset!.status == reportViewModel.Status
                            )
                            && (
                                reportViewModel.Category == null
                                || a.Asset!.Category == reportViewModel.Category
                            )
                    )
                    .Select(a => a.Asset!)
                    .ToList();
            }

            ViewBag.Assets = assets;
            return View();
        }

        [HttpGet]
        public IActionResult Print(string assets)
        {
            List<Asset> assetList = JsonConvert.DeserializeObject<List<Asset>>(assets);

            return PartialView("_Print", assetList);
        }
    }
}
