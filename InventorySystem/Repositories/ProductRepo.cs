using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Repositories
{
    public class ProductRepo : IGenericRepo<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Product obj)
        {
            if(obj != null)
            {
                await _context.Products.AddAsync(obj);
                _context.SaveChanges();
            }
        }

        public void Delete(Product product)
        {
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product>? GetAll()
        {
            var result = _context.Products.Include(x=> x.Category).ToList();

            return result;
        }

        public Product? GetById(int id)
        {
            var product = _context.Products.Include(x=> x.Suppliers).Include(x=>x.Category).SingleOrDefault(x=> x.Id == id);

            if (product is null)
            {
                throw new InvalidOperationException($"No product found with ID: {id}");
            }

            return product;
        }

        public void Update(Product product)
        {
            if (product is null)
            {
                throw new InvalidOperationException($"No product found");
            }
            else
            {
                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }


        //public IEnumerable<ProductViewModel>? ConvertToViewModel(IEnumerable<Product>? products)
        //{ 
        //    List<ProductViewModel> productViewModels = new();

        //    if(products is not null)
        //    {
        //        foreach(Product product in products)
        //        {
        //            productViewModels.Add(new()
        //            {
        //                Name = product.Name,
        //                Price = product.Price,
        //                AlertLevel = product.AlertLevel,
        //                Count = product.Count
        //            });
        //        }
        //    }

        //    return null;
        //}
    }
}
