using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class HomePageViewModel
    {
        public List<Product> LastAddProducts;
        public List<Product> ExpierdProducts;
        public int SuppliersCount;
        public int EmployeesCount;
    }
}
