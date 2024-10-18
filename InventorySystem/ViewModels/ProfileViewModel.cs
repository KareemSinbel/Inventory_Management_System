using InventorySystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventorySystem.ViewModels
{
	public class ProfileViewModel
	{
        public ApplicationUser User { get; set; } 


		[DataType(DataType.Password)]
		[DisplayName("Current Password")]
        public string? CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "New Password must be at least 6 characters long.")]
		[DataType(DataType.Password)]
		[DisplayName("New Password")]
        public string? NewPassword { get; set; }
    }
}
