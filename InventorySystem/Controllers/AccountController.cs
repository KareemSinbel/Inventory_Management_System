using InventorySystem.Repositories;
using InventorySystem.ViewModels;
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
            return View();
        }


        [HttpPost]
        public IActionResult Login([Bind(Prefix = "Item1")]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               if(_accountManagerRepo.CheckLogin(model)) 
               {
                   return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
               }
               ModelState.AddModelError(string.Empty, "Incorrect email or password");
            }
       
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorsHtml = $"<ul class='text-danger'>" + string.Join("", errors.Select(e => $"<li>{e}</li>")) + "</ul>";

            return Json(new { success = false, errorsHtml });
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

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorsHtml = $"<ul class='text-danger'>" + string.Join("", errors.Select(e => $"<li>{e}</li>")) + "</ul>";

            return Json(new { success = false, errorsHtml });
        }
    }
}
