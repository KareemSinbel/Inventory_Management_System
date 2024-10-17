using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IFactoryRepository _factoryRepo;

        public ProductController(IFactoryRepository factoryRepo)
        {
            _factoryRepo = factoryRepo;
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var categoryRepo = _factoryRepo.CreateRepositoryMethod<Category>();
            var supplierRepo = _factoryRepo.CreateRepositoryMethod<Supplier>();
            ProductViewModel productsViewModel = new ProductViewModel();

            if(categoryRepo != null) 
            {
                var categories = categoryRepo.GetAll();
                if(categories != null) 
                {
                    productsViewModel.Categories = categories;
                }
            }

            if(supplierRepo != null)
            {
                var suppliers = supplierRepo.GetAll();
                if(suppliers != null)
                {
                    productsViewModel.Product = new Product();
                    productsViewModel.Product.Suppliers = suppliers.ToList();
                }
            }
            
            return View(productsViewModel);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel productViewModel) 
        {                  
            if(ModelState.IsValid) 
            {
                if(productViewModel.CategoryId != -1)
                {
                    var categoryRepo = _factoryRepo.CreateRepositoryMethod<Category>();
                    var supplierRepo = _factoryRepo.CreateRepositoryMethod<Supplier>();
                    if(categoryRepo is not null && supplierRepo is not null)
                    {
                        var category = categoryRepo.GetById(productViewModel.CategoryId);

                        if(category is not null)
                        { 
                            var productRepo = _factoryRepo.CreateRepositoryMethod<Product>();

                            if(productRepo != null) 
                            {
                                Product product = new()
                                {
                                    Name = productViewModel.Product.Name,
                                    AlertLevel = productViewModel.Product.AlertLevel,
                                    Price = productViewModel.Product.Price,
                                    Count = productViewModel.Product.Count,
                                    Category = category,
                                };

                                foreach(var supplier in productViewModel.SuppliersId) 
                                {
                                    var temp = supplierRepo.GetById(supplier);
                                    if(temp is not null) 
                                    {
                                        product.Suppliers.Add(temp);
                                    }
                                }
                                productRepo.AddAsync(product);
                            }
                        }
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Invalid Category", "Choose a category.");
                }
            }            

            return AddProduct();
        }

        public IActionResult ProductList()
        {
            var productRepo = _factoryRepo.CreateRepositoryMethod<Product>();
            if(productRepo != null) 
            {
                var products = productRepo.GetAll();

                return View(products);
            }

            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();
        }
        public IActionResult EditProduct()
        {
            return View();
        }
    }
}
