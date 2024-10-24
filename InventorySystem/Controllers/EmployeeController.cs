using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [ValidateAntiForgeryToken]
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

            var result = new EmployeeAddViewModel(){IdentityRoles = await accountManagerRepo!.GetAllRoles() };

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(EmployeeAddViewModel model)
        {
            if(ModelState.IsValid) 
            { 
                if(_accountManagerRepo is AccountManagerRepo repo)
                { 
                    if (await _accountManagerRepo.CheckSignUpAsync(model.SignUpViewModel, model.Role))
                    {
                        return RedirectToAction("UserLists");
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

            return RedirectToAction("NewUser");
        }

        public async Task<IActionResult> UserLists()
        {
            var usersWithRoles = await _accountManagerRepo.GetAllAsync();
            
            return View(usersWithRoles);
        }

        public async Task<IActionResult> UserEdit(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var employeeRepo = _factoryRepo.CreateRepositoryMethod<Employee>();

            if(employeeRepo is null)
                return NotFound();

            var employee = employeeRepo.GetById((int)id);

            if(employee is null)
                return NotFound();

            var accountRepo = _accountManagerRepo as AccountManagerRepo;

            var role = await accountRepo!.GetUserRolesNames(employee.User);


            var employeeEditViewModel = new EmployeeEditViewModel()
            {
                User = employee.User,
                Role = role[0].ToString(),
                IdentityRoles = await accountRepo!.GetAllRoles() 
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(EmployeeEditViewModel model)
        {
            var accountRepo = _accountManagerRepo as AccountManagerRepo;  

            if(ModelState.IsValid)
            {

                var originalUser = await accountRepo!.GetUserFullDataByUserNameAsync(model.OriginalUserName);

                 if(originalUser != null)
                 {
                        var originalRoleName = await accountRepo.GetUserRolesNames(originalUser);

                        if(model.Role != originalRoleName[0] && model.Role is not null)
                        { 
                            await accountRepo.UpdateUserRole(originalUser, originalRoleName[0], model.Role);
                        }

                        if(!DataManager.CompareObjects(originalUser, model.User, ["FirstName", "LastName", "Email", "PhoneNumber", "UserName"]))
                        {                            
                            var newUser = await accountRepo.UpdateUserAsync(originalUser, model.User);

                            if (newUser != null) 
                            { 
                                model.User = newUser; 
                            }
                        }
                 }
            }

            model.IdentityRoles = await accountRepo!.GetAllRoles() ;

            return View(model);
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
