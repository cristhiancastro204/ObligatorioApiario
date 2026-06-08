using Microsoft.AspNetCore.Mvc;

namespace ObligatorioApiario.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login (To be implemented later with actual logic)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // For now, just redirect to Home/Index to simulate a successful login
            return RedirectToAction("Index", "Home");
        }
    }
}
