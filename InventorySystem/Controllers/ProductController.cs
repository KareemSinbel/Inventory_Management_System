using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IFactoryRepository _factoryRepo;
        private readonly IGenericRepo<Product> _productRepo;

        public ProductController(IFactoryRepository factoryRepo, IGenericRepo<Product> productRepo)
        {
            _factoryRepo = factoryRepo;
            _productRepo = productRepo; 
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var categoryRepo = _factoryRepo.CreateRepositoryMethod<Category>();
            var supplierRepo = _factoryRepo.CreateRepositoryMethod<Supplier>();
            var stockReportRepo = _factoryRepo.CreateRepositoryMethod<StockReport>();
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
                    productsViewModel.Suppliers = suppliers;
                }
            }
            
            return View(productsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                            Product product = new()
                            {
                                Name = productViewModel.Product.Name,
                                AlertLevel = productViewModel.Product.AlertLevel,
                                Price = productViewModel.Product.Price,
                                Count = productViewModel.Product.Count,
                                Category = category,
                            };

                            foreach(var supplierId in productViewModel.SuppliersId) 
                            {
                                var supplier = supplierRepo.GetById(supplierId);
                                if(supplier is not null) 
                                {
                                    product.Suppliers.Add(supplier);
                                }
                            }
                            _productRepo.AddAsync(product);
                            
                            

                            return Json(new { success = true });
                        }
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Invalid Category", "Choose a category.");
                }
            }            

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            return Json(new { success = false, message = errors });
        }

        public IActionResult ProductList()
        {
            var products = _productRepo.GetAll();

            return View(products);
        }

        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult EditProduct(int? id)
        {
            if (id is null || id == 0)
                return NotFound();


            var product = _productRepo.GetById((int)id);

            if(product is null)
                return NotFound();


            ProductViewModel productsViewModel = new ProductViewModel();

            productsViewModel.Product = product;
          
            var categoryRepo = _factoryRepo.CreateRepositoryMethod<Category>();

            if(categoryRepo != null) 
            {
                var categories = categoryRepo.GetAll();
                if(categories != null) 
                {
                    productsViewModel.Categories = categories;
                }
            }

            var supplierRepo = _factoryRepo.CreateRepositoryMethod<Supplier>();

            if(supplierRepo != null)
            {
                var suppliers = supplierRepo.GetAll();
                if(suppliers != null)
                {
                    productsViewModel.Suppliers = suppliers;
                }
            }


            return View(productsViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid) 
            {
                if(productViewModel.CategoryId != -1)
                {
                    var product = _productRepo.GetById(productViewModel.Product.Id);

                    if (product is null)
                        return NotFound();


                    if(product.Category.Id != productViewModel.CategoryId)
                    { 
                        var categoryRepo = _factoryRepo.CreateRepositoryMethod<Category>();
                        var category = categoryRepo!.GetById(productViewModel.CategoryId);
                        if (category is not null)
                            product.Category = category;
                    }


                    var notSelectedSuppliers = product.Suppliers.Select(x => x.Id).Except(productViewModel.SuppliersId).ToList();
                    var additionalSelectedSuppliers = productViewModel.SuppliersId.Except(product.Suppliers.Select(x=>x.Id)).ToList();

                    if (notSelectedSuppliers.Count > 0) 
                    {
                        foreach(var supplierIdToRemove in notSelectedSuppliers)
                        {
                            var supplier = product.Suppliers.SingleOrDefault(s=> s.Id == supplierIdToRemove);
                            if(supplier is not null)
                                product.Suppliers.Remove(supplier);
                        }
                    }

                    if(additionalSelectedSuppliers.Count > 0)
                    { 
                        var supplierRepo = _factoryRepo.CreateRepositoryMethod<Supplier>();

                        foreach(var supplierIdToAdd in additionalSelectedSuppliers) 
                        {
                            var supplier = supplierRepo!.GetById(supplierIdToAdd);
                            if(supplier is not null)
                                product.Suppliers.Add(supplier);
                        }

                    }

                    _productRepo.Update(product);
                    return RedirectToAction("ProductList");                               
                }
                else
                {
                    ModelState.AddModelError("Invalid Category", "Choose a category.");
                }
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int? id)
        {
            if(id is null || id == 0)
                return NotFound();

            var product = _productRepo.GetById((int)id);

            if(product is null)
                return NotFound();

            _productRepo.Delete(product);
            return RedirectToAction("ProductList");
        }

    }
}
