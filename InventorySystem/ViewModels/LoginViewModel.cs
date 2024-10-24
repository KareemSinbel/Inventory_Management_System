using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.ViewModels
{
    public class LoginViewModel
    {
		[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DataType(DataType.EmailAddress)]
		[DisplayName("Email Address")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}
