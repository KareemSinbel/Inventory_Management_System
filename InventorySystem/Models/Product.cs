using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Product Name is required")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DisplayName("Price")]
        public double Price { get; set; }    

        [Required(ErrorMessage = "Alert Level is required")]
        [DisplayName("Alert Level")]
        public int AlertLevel { get; set; }

        [Required(ErrorMessage = "Product Count is required")]
        [DisplayName("Product Count")]
        public int Count { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Category is required")]
        [ValidateNever] 
        public Category Category { get; set; }

        [Required(ErrorMessage = "Suppliers is required")]
        [DisplayName("Suppliers")]
        [ValidateNever]
        public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public List<AlertReport>? AlertReports { get; set; }
        public List<StockReport>? StockReports { get; set; }
        public string? Description { get; set; }       
    }
}
