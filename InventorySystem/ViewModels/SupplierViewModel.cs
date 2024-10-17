using InventorySystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventorySystem.ViewModels
{
    public class SupplierViewModel
    {
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
    }
}
