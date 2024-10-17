using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IFactoryRepository _factoryRepo;
        private readonly IAccountManagerRepo _accountManagerRepo;

        public EmployeeController(IFactoryRepository factoryRepository, IAccountManagerRepo accountManagerRepo)
        {
            _factoryRepo = factoryRepository;
            _accountManagerRepo = accountManagerRepo;
        }

        public IActionResult Profile()
        {
            var employeeRepo = _factoryRepo.CreateRepositoryMethod<Employee>() as EmployeeRepo;

            if(User.Identity!.IsAuthenticated)
			{
                var accountManagerRepo = _accountManagerRepo as AccountManagerRepo;

				var user =  accountManagerRepo!.GetUserByUserName(User.Identity.Name!).Result;

				if (user != null)
				{
                    return View(employeeRepo!.RetrieveEmployeeData(user.Id));
				}

            }               
            return View();
        }
    }
}
