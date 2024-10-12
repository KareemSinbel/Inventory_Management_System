using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    
}
