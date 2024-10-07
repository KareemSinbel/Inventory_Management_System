using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class StockReport : IReport
    {
        public int Id { get; set; }

        [Required]
        public required Product Product { get; set; }

        [Required]
        public required Supplier Supplier { get; set; }

        [Required]
        public required Employee Employee { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required ActionType Type { get; set; }

        [Required]
        public required int ProductQuantity { get; set; }
    }
}
