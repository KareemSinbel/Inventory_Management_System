using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required bool IsAdmin { get; set; }


        public List<StockReport>? StockReports { get; set; }
    }
}
