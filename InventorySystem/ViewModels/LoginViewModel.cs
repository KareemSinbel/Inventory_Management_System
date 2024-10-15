using System.ComponentModel.DataAnnotations;

namespace InventorySystem.ViewModels
{
    public class LoginViewModel
    {
		[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		public string Password { get; set; }
    }
}
