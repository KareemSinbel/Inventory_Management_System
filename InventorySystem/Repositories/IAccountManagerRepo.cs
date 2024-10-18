using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Repositories
{
	public interface IAccountManagerRepo
	{
		Task<bool> CheckLoginAsync(LoginViewModel model);
		Task<bool> CheckSignUpAsync(SignUpViewModel model);
	}
}