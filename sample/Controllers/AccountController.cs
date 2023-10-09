using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
namespace sample.Controllers
{
    public class AccountController:Controller
    {
        // Other actions and methods

        // Logout action
        public async Task<IActionResult> Logout()
        {
            // Sign out the current user
            await HttpContext.SignOutAsync();

            // Redirect to the home page or any other desired page
            return RedirectToAction("Index", "Home");
        }

    }
}
