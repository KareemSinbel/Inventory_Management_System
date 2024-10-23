using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventorySystem.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> NewUser()
        {
            var accountManagerRepo = _accountManagerRepo as AccountManagerRepo;

            var result = new EmployeeViewModel(){IdentityRoles = await accountManagerRepo!.GetAllRoles() };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> NewUser(EmployeeViewModel model)
        {
            if(ModelState.IsValid) 
            { 
                if(_accountManagerRepo is AccountManagerRepo repo)
                { 
                    if (await _accountManagerRepo.CheckSignUpAsync(model.SignUpViewModel))
                    {
                        return View("UserLists");
                    }
                    else
                    {
                        if(repo.IdentityResult is not null) 
                        {
                            foreach (var error in repo.IdentityResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }                
                }
            }

            return await NewUser();
        }

        public async Task<IActionResult> UserLists()
        {
            var usersWithRoles = await _accountManagerRepo.GetAllAsync();
            
            return View(usersWithRoles);
        }
        public IActionResult UserEdit()
        {
            return View();
        }

        public IActionResult DeleteUser(int? id)
        {
            if(id is null || id == 0)
                return NotFound();

            var employeeRepo = _factoryRepo.CreateRepositoryMethod<Employee>() as EmployeeRepo;

            if(employeeRepo is null)
                return NotFound(nameof(employeeRepo));

            var employee = employeeRepo.GetById((int)id);

            if(employee is null)
                return NotFound();

            employeeRepo.Delete(employee);

            return RedirectToAction("UserLists");
        }
    }
}
