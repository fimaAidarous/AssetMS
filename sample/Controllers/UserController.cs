using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sample.Data;
using sample.Models;

namespace sample.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext context = null)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return _context.Users != null
                ? View(await _context.Users.ToListAsync())
                : Problem("Entity set 'AppDbContext.Asset");
            // return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Role,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Asset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind(
                "Id,Name,Category,Location,status,PurchaseDate,PurchaseCost,PurchaseCost,WarrantyExpiryDate,CreatedAt"
            )]
                Asset asset
        )
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

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.Id == id);
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
