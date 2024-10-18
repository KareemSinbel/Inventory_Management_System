using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace InventorySystem.Repositories
{
	public class AccountManagerRepo : IAccountManagerRepo
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
		private ApplicationUser? _currentUser;
		public IdentityResult? IdentityResult;
		private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountManagerRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			ApplicationDbContext context, IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_authenticationService = authenticationService;
			_httpContextAccessor = httpContextAccessor;
		}

		public ApplicationUser? LoadUserData()
		{
			return _currentUser;
		}

		public async Task<bool> ChangePassword(ApplicationUser user, string currentPass, string newPass)
		{
			var result = await _userManager.ChangePasswordAsync(user, currentPass, newPass);

			if(result.Succeeded) 
			{
				return true;
			}

			IdentityResult = result;
	
			return false;
		}

		public async Task<ApplicationUser?> GetUserFullDataByUserNameAsync(string userName)
		{ 
			var user = await _context.Users.Include(x=> x.Employee).SingleOrDefaultAsync(x=> x.UserName == userName);

			return user?? null;
		}

		public async Task<bool> CheckLoginAsync(LoginViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is not null)
			{
				if (await _userManager.CheckPasswordAsync(user, model.Password))
				{
					var Claims = new List<Claim>
                    {
                        new("FirstName", user.FirstName),
                        new("LastName", user.LastName)
                    };

					//var claimsIdentity = new ClaimsIdentity(Claims, "login");
					//_httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)).GetAwaiter().GetResult();

					var claimsResult = await _userManager.AddClaimsAsync(user, Claims);

					if(claimsResult.Succeeded)
					{ 
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
						if (result.Succeeded)
						{
							_currentUser = user;
							return true;
						}
					}
				}
			}
			return false;
		}


		public async Task<bool> CheckSignUpAsync(SignUpViewModel model)
		{
			var user = new ApplicationUser{
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
					Employee = new Employee { Name = model.FirstName + " " + model.LastName, IsAdmin = false }
            };      
		    

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
				//_context.Employees.Add(new Employee{Name = user.FirstName + " " + user.LastName, IsAdmin= false, UserId = user.Id});
				//await _context.SaveChangesAsync();
				await _userManager.AddToRoleAsync(user, RolesType.Role_Employee);
                return true;
            }

			IdentityResult = result;
			return false;
		}


		public async Task<ApplicationUser?> UpdateUserAsync(ApplicationUser user, ApplicationUser newUser)
		{
			if (user != null)
			{
				DataManager.UpdateObjectValuesForSpecificProp(user, newUser, ["FirstName", "LastName", "Email", "PhoneNumber", "UserName"]);

				var result = await _userManager.UpdateAsync(user);
				user.Employee.Name = user.FirstName + " " + user.LastName;


				_context.Employees.Update(user.Employee);
				await _context.SaveChangesAsync();

				if(result.Succeeded) 
				{
					return user;
				}
			}

			return null;
		}
	}
}
