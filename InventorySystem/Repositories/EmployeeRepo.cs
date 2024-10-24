using InventorySystem.Data;
using InventorySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Repositories
{
	public class EmployeeRepo : IGenericRepo<Employee>
	{
        private readonly ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

		public async Task AddAsync(Employee obj)
		{
			if(obj is not null) 
			{
				await _context.Employees.AddAsync(obj);
				await _context.SaveChangesAsync();
			}
		}

		public void Delete(Employee emp)
		{
			_context.Employees.Remove(emp);
			_context.Users.Remove(emp.User);
			_context.SaveChanges();
		}

		public IEnumerable<Employee>? GetAll()
		{
			return _context.Employees.ToList();
		}

		public Employee? GetById(int id)
		{
			var employee = _context.Employees.Include(x=>x.User).SingleOrDefault(x=> x.Id == id);

			if(employee is null) 
			{
				return null;
			}

			return employee;
		}

		public Employee? RetrieveEmployeeData(string id)
        {
            var employee = _context.Employees.Where(x=> x.UserId == id).FirstOrDefault();

            return employee;
        }

		public void Update(Employee obj)
		{
			_context.Employees.Update(obj);
            _context.SaveChanges();
		}
	}
}
