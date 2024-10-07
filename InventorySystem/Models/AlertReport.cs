
using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class AlertReport : IReport
    {
        public int Id { get; set; }

        [Required]
        public required Product ProductId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        [Required]
        public required AlertStatus Status { get; set; }
    }
}
