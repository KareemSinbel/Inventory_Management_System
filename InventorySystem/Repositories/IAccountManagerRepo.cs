using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventorySystem.Repositories
{
	public interface IAccountManagerRepo
	{
		bool CheckLogin(LoginViewModel model);
		bool CheckSignUp(SignUpViewModel model);
	}
}