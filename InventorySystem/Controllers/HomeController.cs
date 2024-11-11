using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
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
        private readonly IHomeRepo _repo;

        public HomeController(ILogger<HomeController> logger, IHomeRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            var HomeVM = new HomePageViewModel
            {
                LastAddProducts = _repo.productLastAdd(),
                ExpierdProducts = _repo.ExpiredProducts(),
                EmployeesCount = _repo.EmpolyeeCount(),
                SuppliersCount = _repo.SupplierCount(),
                CategoriesCount = _repo.CategoriesCount(),
                ProductsCount = _repo.ProductsCount()
            };
            return View(HomeVM);
        }

        public IActionResult GeneralSettings()
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
