using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
