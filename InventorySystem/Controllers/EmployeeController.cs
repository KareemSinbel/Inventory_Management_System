using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
