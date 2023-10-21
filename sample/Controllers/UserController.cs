using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample.Data;
using sample.Migrations;
using sample.Models;
using sample.Models.DTO;
using sample.Repositories.Implementation;
using sample.Repositories.Interfaces;
namespace sample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserAuthenticationService _service;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserAuthenticationService service)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
        }

        // GET: User

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersListDTO = users.Select(u => new UserListDTO
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.UserName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result,
            }).ToList();

            return View(usersListDTO);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.ToListAsync();
            var detailedUser = users.Select(a => new UserListDTO
            {
                Id = a.Id,
                Name = a.Name,
                Username = a.UserName,
                Email = a.Email,
                Roles = _userManager.GetRolesAsync(a).Result
            }).FirstOrDefault(a => a.Id == id);
            if (detailedUser == null)
            {
                return NotFound();
            }

            return View(detailedUser);
        }

        // GET: user/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                var role = await _userManager.GetRolesAsync(user);
                var model = new UserEditModel()
                {
                    Id = user.Id,
                    Email = user!.Email,
                    Username = user!.UserName,
                    Name = user.Name,
                    Role = string.Join(",", role)

                };
                return View(model);
            }
            return Problem();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name, Username, Email, Role, CurrentPassword, Password, PasswordConfirm")] UserEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.UpdateAsync(id, model);
            if (result.StatusCode == 0)
            {
                TempData["msg"] = result.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Username, Email, Role, Password, PasswordConfirm")] RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.RegistrationAsync(model);
            if (result.StatusCode == 0)
            {
                TempData["msg"] = result.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }



        // // GET: User/Edit/5    
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null || _context.Users == null)
        //     {
        //         return NotFound();
        //     }

        //     var User = await _context.Users.FindAsync(id);
        //     if (User == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(User);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("Name,Email,Role,Password")] User user)
        // {
        //     if (id != user.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(user);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!UserExists(user.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(user);
        // }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var users = await _context.Users.ToListAsync();
            var detailedUser = users.Select(a => new UserListDTO
            {
                Id = a.Id,
                Name = a.Name,
                Username = a.UserName,
                Email = a.Email,
                Roles = _userManager.GetRolesAsync(a).Result
            }).FirstOrDefault(a => a.Id == id);
            if (detailedUser == null)
            {
                return NotFound();
            }
            return View(detailedUser);

        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            // 
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // private bool UserExists(int id)
        // {
        //     return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        // }

        // // GET: User/Print/5
        // public IActionResult Print(int id)
        // {
        //     var User = _context.Users.FirstOrDefault(m => m.Id == id);
        //     return PartialView("_Print", User);
        // }

    }
}
