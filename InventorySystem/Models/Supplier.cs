using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Models
{
    public class Supplier
    {
        [Key]
        public  int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [DisplayName("Supplier Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		[Phone(ErrorMessage = "Invalid Phone Number")]
		[RegularExpression(@"(01[0125])[0-9]{8}", ErrorMessage = "Invalid Phone Number.")]
        public string? ContactInfo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [DisplayName("Supplier Address")]
        public string? Address { get; set; }

        [ValidateNever]
        public List<Product> Products { get; set; }

    }
}
