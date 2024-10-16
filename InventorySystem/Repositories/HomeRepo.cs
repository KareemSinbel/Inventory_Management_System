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
    }
}
