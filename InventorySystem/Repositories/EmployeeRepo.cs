using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Repositories
{
	public class EmployeeRepo : IGenericRepo<Employee>
	{
        private readonly ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

		public Task AddAsync(Employee obj)
		{
			throw new NotImplementedException();
		}

		public void DeleteById(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Employee>? GetAll()
		{
			throw new NotImplementedException();
		}

		public Employee? GetById(int id)
		{
			throw new NotImplementedException();
		}

		public Employee? RetrieveEmployeeData(string id)
        {
            var employee = _context.Employees.Where(x=> x.UserId == id).FirstOrDefault();

            return employee;
        }

		public void Update(Employee obj)
		{
			throw new NotImplementedException();
		}
	}
}
