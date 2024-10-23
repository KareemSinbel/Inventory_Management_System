using InventorySystem.Data;
using InventorySystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Repositories
{
    public class SupplierRepo : IGenericRepo<Supplier>
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(Supplier supplier)
        {
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Supplier>? GetAll()
        {
            var result = _context.Suppliers.ToList();

            return result;
        }

        public Supplier? GetById(int id)
        {
            var supplier = _context.Suppliers.Find(id);

            if (supplier is null)
            {
                throw new InvalidOperationException($"No supplier found with ID: {id}");
            }

            return supplier;
        }

        public void Update(Supplier supplier)
        {
            //var supplier = _context.Suppliers.Find(obj.Id);
            if (supplier is null)
            {
                throw new InvalidOperationException($"No supplier found with ID: {supplier.Id}");
            }
            else
            {
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
            }
        }

        public async Task AddAsync(Supplier obj)
        {
            if(obj is not null)
            {
                await _context.Suppliers.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
        }
    }
}
