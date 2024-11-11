using InventorySystem.Models;

namespace InventorySystem.Repositories
{
    public interface IHomeRepo
    {
        Task<Employee?> GetEmployeeAsync(string userId);
        Task<ApplicationUser?> GetUserIdentityAsync(string userName);
        public int EmpolyeeCount();
        public int SupplierCount();
        public int CategoriesCount();
        public int ProductsCount();
        public List<Product> productLastAdd();
        public List<Product> ExpiredProducts();

    }
}