using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Category Name is required")]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        public List<Product>? Products { get; set; }
    }
}
