using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class StockReport : IReport
    {
        public int Id { get; set; }

        [Required]
        public required Product ProductId { get; set; }

        [Required]
        public required Supplier SupplierId { get; set; }

        [Required]
        public required Employee EmployeeId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required ActionType Type { get; set; }

        [Required]
        public required int ProductQuantity { get; set; }
    }
}
