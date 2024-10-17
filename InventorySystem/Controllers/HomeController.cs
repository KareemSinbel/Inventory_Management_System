using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventorySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult GeneralSettings()
        {
            return View();
        }
        
        public IActionResult ProductList()
        {
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        public IActionResult ProductDetails()
        {
            return View();
        }
        public IActionResult EditProduct()
        {
            return View();
        }
        public IActionResult CategoryList()
        {
            return View();
        }
        public IActionResult EditCategory()
        {
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        public IActionResult SalesList()
        {
            return View();
        }
        public IActionResult AddSales()
        {
            return View();
        }
        public IActionResult TransferList()
        {
            return View();
        }
        public IActionResult AddTransfer()
        {
            return View();
        }
        public IActionResult EditTransfer()
        {
            return View();
        }
        public IActionResult CustomerList()
        {
            return View();
        }
        public IActionResult AddCustomer()
        {
            return View();
        }
        public IActionResult SupplierList()
        {
            return View();
        }
        public IActionResult AddSupplier()
        {
            return View();
        }
        public IActionResult EditCustomer()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
