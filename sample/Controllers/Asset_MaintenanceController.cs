using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Models;

namespace sample.Controllers;

public class Asset_MaintenanceController : Controller
{
    private readonly AppDbContext _context;

    public Asset_MaintenanceController(AppDbContext context)
    {
        _context = context;
    }

    // GET: AssetMaintenances

    public async Task<IActionResult> Index()
    {
        return _context.AssetMaintenances != null
            ? View(await _context.AssetMaintenances.ToListAsync())
            : Problem("Entity set 'AppDbContext.AssetMaintenances'  is null.");
    }

    // GET: AssetMaintenances/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.AssetMaintenances == null)
        {
            return NotFound();
        }

        var AssetMaintenances = await _context.AssetMaintenances.FirstOrDefaultAsync(
            m => m.Id == id
        );
        if (AssetMaintenances == null)
        {
            return NotFound();
        }

        return View(AssetMaintenances);
    }

    // GET: AssetMaintenances/Create

    public IActionResult Create()
    {
        var model = new Asset_Maintenance();
        var assets = _context.Assets.ToList();
        Console.WriteLine(assets);

        // Replace `_context.Assets.ToList()` with your logic to retrieve the assets list

        var assetList = assets
            .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
            .ToList();
        ViewBag.Assets = assetList;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,AssetId,MaintenanceDate,MaintenanceType,Description,Technician,Cost")]
            Asset_Maintenance asset_Maintenance
    )
    {
        if (ModelState.IsValid)
        {
            asset_Maintenance.CreatedAt = DateTime.Now;
            _context.Add(asset_Maintenance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(asset_Maintenance);
    }

    // GET: AssetMaintenances/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.AssetMaintenances == null)
        {
            return NotFound();
        }

        var assetMain = await _context.AssetMaintenances.FindAsync(id);
        if (assetMain == null)
        {
            return NotFound();
        }
        return View(assetMain);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,AssetId,MaintenanceDate,MaintenanceType,Description,Technician,Cost,CreatedAt")]
            Asset_Maintenance asset_Maintenance
    )
    {
        if (id != asset_Maintenance.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(asset_Maintenance);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetsMainExists(asset_Maintenance.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(asset_Maintenance);
    }

    // GET: AssetMaintenances/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.AssetMaintenances == null)
        {
            return NotFound();
        }

        var assetMain = await _context.AssetMaintenances.FirstOrDefaultAsync(m => m.Id == id);
        if (assetMain == null)
        {
            return NotFound();
        }

        return View(assetMain);
    }

    // POST: AssetMaintenances/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.AssetMaintenances == null)
        {
            return Problem("Entity set 'AppDbContext.AssetMaintenances'  is null.");
        }
        var assetMain = await _context.AssetMaintenances.FindAsync(id);
        if (assetMain != null)
        {
            _context.AssetMaintenances.Remove(assetMain);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AssetsMainExists(int id)
    {
        return (_context.AssetMaintenances?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public ActionResult MaintenanceIndex()
    {
        var model = _context.AssetMaintenances.Include("Asset").ToList();
        return View(model);
    }

    // GET: Asset/Print/5
    public IActionResult Print(int id)
    {
        var assetMain = _context.AssetMaintenances.FirstOrDefault(m => m.Id == id);
        return PartialView("_Print", assetMain);
    }
}
