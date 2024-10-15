using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }


        [Required]
        public required bool IsAdmin { get; set; }


        public List<StockReport>? StockReports { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
