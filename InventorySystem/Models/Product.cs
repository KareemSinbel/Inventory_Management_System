using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required double Price { get; set; }    

        [Required]
        public required int AlertLevel { get; set; }

        [Required]
        public required int Count { get; set; }

        [Required]
        public required Category Category { get; set; }

        [Required]
        public required List<Supplier> Suppliers { get; set; }

        public List<AlertReport>? AlertReports { get; set; }
        public List<StockReport>? StockReports { get; set; }
        public string? Description { get; set; }
    }
}
