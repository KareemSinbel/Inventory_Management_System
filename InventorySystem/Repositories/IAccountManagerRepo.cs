using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Repositories
{
	public interface IAccountManagerRepo
	{
		Task<bool> CheckLoginAsync(LoginViewModel model);
		Task<bool> CheckSignUpAsync(SignUpViewModel model, string? role = null);
		Task<IEnumerable<EmployeeListViewModel>?> GetAllAsync();

		Task<ApplicationUser?> UpdateUserAsync(ApplicationUser user, ApplicationUser newUser);
	}
}