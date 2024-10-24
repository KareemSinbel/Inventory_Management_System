using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace InventorySystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepo _homeRepo;

        public HomeController(ILogger<HomeController> logger, IHomeRepo homeRepo)
        {
            _homeRepo = homeRepo;
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

        public IActionResult GeneralSettings()
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
