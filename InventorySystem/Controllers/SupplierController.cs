using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
