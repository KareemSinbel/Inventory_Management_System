using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Supplier
    {
        [Key]
        public  int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required List<Product> Products { get; set; }


        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
    }
}
