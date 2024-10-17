using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.ViewModels
{
    public class ProductViewModel
    {
        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<int> SuppliersId { get; set; }

        public int CategoryId { get; set; }

    }
}
