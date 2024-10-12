using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
