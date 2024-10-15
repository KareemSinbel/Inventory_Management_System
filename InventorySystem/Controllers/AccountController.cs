using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        // This action displays the login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // for Only Display the Login Page to Test It 
        // This action handles the login form submission
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Here you would typically check the user credentials against your database
                // For demonstration, let's assume these credentials are valid
                if (model.Email == "admin@example.com" && model.Password == "password123")
                {
                    // Set up user session or authentication
                    // Redirect to a different page after successful login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Invalid credentials
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }


        [HttpPost]
        public IActionResult Signup(/* Add parameters for username, email, phone, and password here */)
        {
            // Implement signup logic here (e.g., save user to database)

            return RedirectToAction("Login"); // Redirect to login after signup
        }

    }
}
