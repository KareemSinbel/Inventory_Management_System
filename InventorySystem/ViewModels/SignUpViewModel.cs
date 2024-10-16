using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.ViewModels
{
	public class SignUpViewModel
	{
		[Required]
		[DisplayName("First Name")]
        public string FirstName { get; set; }

		[Required]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Required]
		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email Address")]
        public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }


		[Required(ErrorMessage = "Confirm password is required.")]
		[Compare("Password", ErrorMessage = "Confirm Password must match the Password")]
		[DataType(DataType.Password)]
		[DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		[Phone(ErrorMessage = "Invalid Phone Number")]
		[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
		public string PhoneNumber { get; set; }
    }
}
