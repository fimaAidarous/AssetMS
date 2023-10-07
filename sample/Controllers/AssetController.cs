using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Models;

namespace sample.Controllers
{
    public class AssetController: Controller
    {
        private readonly AppDbContext _context;

        public AssetController(AppDbContext context)
        {
            _context = context; 
        }

        // GET: Asset

        public async Task<IActionResult> Index()
        {
            return _context.Assets != null ?
                         View(await _context.Assets.ToListAsync()) :
                         Problem("Entity set 'AppDbContext.Asset'  is null.");
        }

        // GET: Asset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assets == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Asset/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Location,status,PurchaseDate,PurchaseCost,PurchaseCost,WarrantyExpiryDate,CreatedAt")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(asset);
        }

        // GET: Asset/Edit/5    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assets == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Location,status,PurchaseDate,PurchaseCost,PurchaseCost,WarrantyExpiryDate,CreatedAt")] Asset asset)
        {
            if (id != asset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetsExists(asset.Id))
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
            return View(asset);
        }

        // GET: Asset/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assets == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // POST: Asset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assets == null)
            {
                return Problem("Entity set 'AppDbContext.Assets'  is null.");
            }
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetsExists(int id)
        {
            return (_context.Assets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Asset/Print/5
        public IActionResult Print(int id)
        {
            var asset = _context.Assets.FirstOrDefault(m => m.Id == id);
            return PartialView("_Print", asset);
        }


    }
}
