using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class HomePageViewModel
    {
        public int EmployeesCount { get; set; }
        public int SuppliersCount { get; set; }
        public int ProductsCount { get; set; }
        public int CategoriesCount { get; set; }
        public List<Product> LastAddProducts { get; set; }
        public List<Product> ExpierdProducts { get; set; }
    }

}
