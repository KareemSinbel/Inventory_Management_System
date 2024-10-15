using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Repositories
{
	public class AccountManagerRepo : IAccountManagerRepo
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
		private ApplicationUser? _currentUser;
        public ApplicationUser? CurrentUser { get{ return _currentUser;} }
		public IdentityResult? IdentityResult;

        public AccountManagerRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		public bool CheckLogin(LoginViewModel model)
		{
			var user = _userManager.FindByEmailAsync(model.Email).Result;

			if (user is not null)
			{
				if (_userManager.CheckPasswordAsync(user, model.Password).Result)
				{
					var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
					if (result.Succeeded)
					{
						_currentUser = user;
						return true;
					}
				}
			}

			return false;
		}


		public bool CheckSignUp(SignUpViewModel model)
		{
			var user = new ApplicationUser{
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
					Employee = new Employee { Name = model.FirstName + " " + model.LastName, IsAdmin = false }
            };      
		    

            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
				//_context.Employees.Add(new Employee{Name = user.FirstName + " " + user.LastName, IsAdmin= false, UserId = user.Id});
				//await _context.SaveChangesAsync();
                return true;
            }

			IdentityResult = result;
			return false;
		}
	}
}
