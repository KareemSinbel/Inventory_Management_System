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

        public void DeleteById(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public List<Supplier> GetAll()
        {
            var result = _context.Suppliers.ToList();
            return result;
        }

        public Supplier GetById(int id)
        {
            var supplier = _context.Suppliers.Find(id);

            if (supplier is null)
            {
                throw new InvalidOperationException($"No supplier found with ID: {id}");
            }

            return supplier;
        }

        public void Update(Supplier obj)
        {
            var supplier = _context.Suppliers.Find(obj.Id);
            if (supplier is null)
            {
                throw new InvalidOperationException($"No supplier found with ID: {obj.Id}");
            }
            else
            {
                supplier.Name = obj.Name;
                supplier.Address = obj.Address;
                supplier.ContactInfo = obj.ContactInfo;
                _context.Suppliers.Add(supplier);
            }
            _context.SaveChanges();
        }
    }
}
