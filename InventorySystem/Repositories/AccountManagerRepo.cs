using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace InventorySystem.Repositories
{
	public class AccountManagerRepo : IAccountManagerRepo
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;
		private ApplicationUser? _currentUser;
		public IdentityResult? IdentityResult;
		private readonly IAuthenticationService _authenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountManagerRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			ApplicationDbContext context, IAuthenticationService authenticationService, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_authenticationService = authenticationService;
			_httpContextAccessor = httpContextAccessor;
			_roleManager = roleManager;
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


		public async Task<bool> CheckSignUpAsync(SignUpViewModel model, string? role = null)
		{
			var user = new ApplicationUser{
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
					Employee = new Employee { Name = model.FirstName + " " + model.LastName, IsAdmin = false, CreatedOn= DateOnly.FromDateTime(DateTime.Now), Status = true },
            };      
		    

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
				//_context.Employees.Add(new Employee{Name = user.FirstName + " " + user.LastName, IsAdmin= false, UserId = user.Id});
				//await _context.SaveChangesAsync();
				await _userManager.AddToRoleAsync(user, role??RolesType.Role_Employee);
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

		public async Task LogOutAsync()
		{
			await _signInManager.SignOutAsync();
		}

        public async Task<IEnumerable<EmployeeViewModel>?> GetAllAsync()
        {
            //var users = from user in _context.Users
            //			join userRole in _context.UserRoles on user.Id equals userRole.UserId
            //			join role in _context.Roles on userRole.RoleId equals role.Id
            //			group role by new {user.Id, user.UserName, user.Email, user.PhoneNumber} into userGroup
            //			select new Us

			var employees = await _context.Employees.Include(x=> x.User).ToListAsync();
            List<EmployeeViewModel> employeeViews = new List<EmployeeViewModel>();

            foreach (var employee in employees) 
			{
				if(employee != null)
				{
					employeeViews.Add(
					new()
					{
						Employee = employee,
						IListEmployeeRoles = await _userManager.GetRolesAsync(employee.User)
					});
				}
					
			}

            return employeeViews;
        }

		public async Task<IEnumerable<IdentityRole>> GetAllRoles()
		{
			return await _roleManager.Roles.ToListAsync();
		}
	}
}
