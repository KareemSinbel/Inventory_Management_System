using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Repositories
{
    public class CategoryRepo : IGenericRepo<Category>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Category obj)
        {
            if(obj != null)
            {
                await _context.Categories.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
        }

        public void Delete(Category category)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category>? GetAll()
        {
            var result = _context.Categories.ToList();

            return result;
        }

        public Category? GetById(int id)
        {
            var result = _context.Categories.SingleOrDefault(x=> x.Id == id);

            return result;
        }

        public void Update(Category obj)
        {
            throw new NotImplementedException();
        }
    }
}
