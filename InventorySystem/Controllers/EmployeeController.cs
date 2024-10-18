using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

				var user =  accountManagerRepo!.GetUserFullDataByUserNameAsync(User.Identity.Name!).Result;

				if (user != null)
				{

                    return View(new ProfileViewModel(){User = user});
                    //return View(employeeRepo!.RetrieveEmployeeData(user.Id));
				}

            }               
            return View();
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel profileViewModel)
        {
            if(ModelState.IsValid)
            { 
                if(profileViewModel.User != null) 
                {
                    var accountRepo = _accountManagerRepo as AccountManagerRepo;  

                    var user =  accountRepo!.GetUserFullDataByUserNameAsync(User.Identity!.Name!).Result;

                    if(user != null)
                    {
                        if(profileViewModel.CurrentPassword != null && profileViewModel.NewPassword != null) 
                        {
                            if (!accountRepo!.ChangePassword(user, profileViewModel.CurrentPassword, profileViewModel.NewPassword).Result)
                            {
                                ModelState.AddModelError("wrong current password", "Current Password is wrong");                               
                            }
                        }

                        if(!DataManager.CompareObjects(user, profileViewModel.User, ["FirstName", "LastName", "Email", "PhoneNumber", "UserName"]))
                        {                            
                            var newUser = accountRepo.UpdateUserAsync(user, profileViewModel.User).Result;

                            if (newUser != null) 
                            { 
                                profileViewModel.User = newUser;   

                                return View(profileViewModel);
                            }
                        }
                    }
                }
            }

            return Profile();
        }
    }
}
