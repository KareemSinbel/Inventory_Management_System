using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InventorySystem.ViewModels
{
    public class EmployeeListViewModel
    {
        [ValidateNever]
        public Employee Employee { get; set; }

        public IList<string>? IListEmployeeRoles { get; set; }
    }
}
