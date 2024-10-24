
using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class AlertReport : IReport
    {
        public int Id { get; set; }

        [Required]
        public required Product Product { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public  AlertStatus Status { get; set; }
    }
}
