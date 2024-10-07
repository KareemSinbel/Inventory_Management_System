using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public interface IReport
    {
        public int Id { get; set; }

        [Required]
        public  Product ProductId { get; set; }

        [Required]
        public  DateTime Date { get; set; }
    }
}
