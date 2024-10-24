using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace InventorySystem.ViewModels
{
    public class EmployeeEditViewModel
    {
        public ApplicationUser User { get; set; }

        [DisplayName("Role")]
		public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<IdentityRole> IdentityRoles { get; set; }

        public string OriginalUserName { get; set; }
    }
}
