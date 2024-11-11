using InventorySystem.Data;
using InventorySystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Repositories
{
    public class HomeRepo : IHomeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<ApplicationUser?> GetUserIdentityAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is not null)
            {
                return user;
            }

            return null;
        }


        public async Task<Employee?> GetEmployeeAsync(string userId)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(x => x.UserId == userId);
            // var temp = await _context.Users.Include(x=> x.Employee).SingleOrDefaultAsync(x=> x.Id == userId);

            if (employee is not null)
            {
                return employee;
            }

            return null;
        }

        public int EmpolyeeCount()
        {
            return _context.Employees.Count();
        }
        public int SupplierCount()
        {
            return _context.Suppliers.Count();
        }
        public List<Product> productLastAdd()
        {
            var LastAdd = _context.Products.Include(x => x.Suppliers).Include(x => x.Category).OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            return LastAdd;
        }
        public List<Product> ExpiredProducts()
        {
            var ExpiredProducts = _context.Products.Include(x => x.Suppliers).Include(x => x.Category).Where(x => x.AlertLevel > x.Count).ToList();
            return ExpiredProducts;
        }
    }
}
