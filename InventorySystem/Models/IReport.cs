using InventorySystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public interface IReport
    {
        public int Id { get; set; }

        [Required]
        public  Product Product { get; set; }

        [Required]
        public  DateTime Date { get; set; }
    }
}
