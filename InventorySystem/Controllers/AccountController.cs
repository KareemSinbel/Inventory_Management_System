using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountManagerRepo _accountManagerRepo;
        public AccountController(IAccountManagerRepo accountManagerRepo)
        {
            _accountManagerRepo = accountManagerRepo;
        }


        // This action displays the login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // for Only Display the Login Page to Test It 
        // This action handles the login form submission
        [HttpPost]
        public IActionResult Login([Bind(Prefix = "Item1")]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               if(_accountManagerRepo.CheckLogin(model)) 
               {
                    return RedirectToAction("Index", "Home");
               }
            }

            // If we got this far, something failed, redisplay the form
            var tupleModel = new Tuple<LoginViewModel, SignUpViewModel>(model, new SignUpViewModel());
            ModelState.AddModelError(string.Empty, "Invalid email or password.");

            return View(tupleModel);
        }



        [HttpPost]
        public IActionResult SignUp([Bind(Prefix = "Item2")]SignUpViewModel model)
        {
            if(ModelState.IsValid) 
            {
                if(_accountManagerRepo is AccountManagerRepo repo)
                { 
                    if (_accountManagerRepo.CheckSignUp(model))
                    {
                        return RedirectToAction("Login");
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

            return View("Login");
        }

    }
}
