using System.ComponentModel.DataAnnotations;

namespace InventorySystem.ViewModels
{
	public class SignUpViewModel
	{
		[Required]
        public  string FirstName { get; set; }

		[Required]
		public  string LastName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public  string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		public  string Password { get; set; }

		[Required]
		public  string UserName { get; set; }
    }
}
