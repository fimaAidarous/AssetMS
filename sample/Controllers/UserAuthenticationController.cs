using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sample.Models.DTO;
using sample.Repositories.Interfaces;

namespace sample.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;

        public UserAuthenticationController(IUserAuthenticationService service)
        {
            _service = service;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }
            registration.Role = "Admin";
            var result = await _service.RegistrationAsync(registration);
            if (result.StatusCode == 1)
                return RedirectToAction(nameof(Login));
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1)
                return RedirectToAction("Index", "Home");
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.logOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
