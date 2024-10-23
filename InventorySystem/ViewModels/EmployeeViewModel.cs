using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace InventorySystem.ViewModels
{
    public class EmployeeViewModel
    {
        [ValidateNever]
        public Employee Employee { get; set; }

        public IList<string>? IListEmployeeRoles { get; set; }

        public SignUpViewModel SignUpViewModel { get; set; }

        [DisplayName("Role")]
		public string? Role { get; set; }

        [ValidateNever]
        public IEnumerable<IdentityRole> IdentityRoles { get; set; }
    }
}
