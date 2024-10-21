using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult InventoryReport()
        {
            return View();
        }
        public IActionResult SalesReport()
        {
            return View();
        }
        public IActionResult InvoiceReport()
        {
            return View();
        }
        public IActionResult SupplierReport()
        {
            return View();
        }
        public IActionResult CustomerReport()
        {
            return View();
        }
    }
    
}
