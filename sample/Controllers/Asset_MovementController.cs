using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Models;

namespace sample.Controllers
{
    [Authorize(Roles ="Admin")]
    public class Asset_MovementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Asset_MovementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetMovement

        public async Task<IActionResult> Index()
        {
            return _context.AssetMovements != null
                ? View(await _context.AssetMovements.ToListAsync())
                : Problem("Entity set 'AppDbContext.AssetMovements'  is null.");
        }

        // GET: AssetMovement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssetMovements == null)
            {
                return NotFound();
            }

            var AssetMovement = await _context.AssetMovements.FirstOrDefaultAsync(m => m.Id == id);
            if (AssetMovement == null)
            {
                return NotFound();
            }

            return View(AssetMovement);
        }

        // GET: AssetMovement/Create
        public IActionResult Create()
        {
            var model = new Asset_Movement();
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
            [Bind("Id,AssetId,FromLocation,ToLocation,MoveDate,MoveReason,ResponsibleParty")]
                Asset_Movement asset_Movement
        )
        {
            if (ModelState.IsValid)
            {
                asset_Movement.CreatedAt = DateTime.Now;
                _context.Add(asset_Movement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asset_Movement);
        }

        // GET: AssetMovement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AssetMovements == null)
            {
                return NotFound();
            }

            var assetMove = await _context.AssetMovements.FindAsync(id);
            if (assetMove == null)
            {
                return NotFound();
            }
            return View(assetMove);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,AssetId,FromLocation,ToLocation,MoveDate,MoveReason,ResponsibleParty")]
                Asset_Movement asset_Movement
        )
        {
            if (id != asset_Movement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset_Movement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetsMoveExists(asset_Movement.Id))
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
            return View(asset_Movement);
        }

        // GET: AssetMovement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AssetMovements == null)
            {
                return NotFound();
            }

            var assetMove = await _context.AssetMovements.FirstOrDefaultAsync(m => m.Id == id);
            if (assetMove == null)
            {
                return NotFound();
            }

            return View(assetMove);
        }

        // POST: AssetMovement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AssetMovements == null)
            {
                return Problem("Entity set 'AppDbContext.AssetMovements'  is null.");
            }
            var assetMove = await _context.AssetMovements.FindAsync(id);
            if (assetMove != null)
            {
                _context.AssetMovements.Remove(assetMove);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetsMoveExists(int id)
        {
            return (_context.AssetMovements?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public ActionResult MovementIndex()
        {
            var model = _context.AssetMovements.Include("Asset").ToList();
            return View(model);
        }

        public IActionResult Print(int id)
        {
            var assetMove = _context.AssetMovements.FirstOrDefault(m => m.Id == id);
            return PartialView("_Print", assetMove);
        }
    }
}
