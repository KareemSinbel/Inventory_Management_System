using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace InventorySystem.Controllers
{
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
            var user = _homeRepo.GetUserIdentityAsync(User.Identity.Name).GetAwaiter().GetResult();
            var employee = _homeRepo.GetEmployeeAsync(user.Id).GetAwaiter().GetResult();

            //if (TempData.TryGetValue("UserData", out var userData))
            //{
            //    var userDataString = userData as string;
            //    if (!string.IsNullOrEmpty(userDataString))
            //    {
            //        var user = JsonConvert.DeserializeObject<Employee>(userDataString);

            //        return View(user);
            //    }
            //}


            return View(employee);
        }

        public IActionResult Privacy()
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
