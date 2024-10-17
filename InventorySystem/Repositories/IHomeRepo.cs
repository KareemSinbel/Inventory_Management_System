using InventorySystem.Models;

namespace InventorySystem.Repositories
{
    public interface IHomeRepo
    {
        Task<Employee?> GetEmployeeAsync(string userId);
        Task<ApplicationUser?> GetUserIdentityAsync(string userName);
    }
}