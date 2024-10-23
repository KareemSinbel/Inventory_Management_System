using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
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


        [HttpGet]
        public IActionResult Login()
        {
            if(User is not null && User.Identity.IsAuthenticated)
            { 
                return RedirectToAction("Index","Home");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([Bind(Prefix = "Item1")]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(_accountManagerRepo is AccountManagerRepo repo)
                { 
                    if(await repo.CheckLoginAsync(model)) 
                    {
                        return Json(new { success = true,redirectUrl = Url.Action("Index", "Home")});
                    }
                    ModelState.AddModelError(string.Empty, "Incorrect email or password");
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            return Json(new { success = false, errors});
        }



        [HttpPost]
        public async Task<IActionResult> SignUp([Bind(Prefix = "Item2")]SignUpViewModel model)
        {
            if(ModelState.IsValid) 
            {
                if(_accountManagerRepo is AccountManagerRepo repo)
                { 
                    if (await _accountManagerRepo.CheckSignUpAsync(model))
                    {
                        return Json(new { success = true,redirectUrl = Url.Action("Login")});
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

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            return Json(new { success = false, errors });
        }


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if(_accountManagerRepo is AccountManagerRepo repo)
            {
                await repo.LogOutAsync();
            }

            return RedirectToAction("Login");
        }
               
    }
}
