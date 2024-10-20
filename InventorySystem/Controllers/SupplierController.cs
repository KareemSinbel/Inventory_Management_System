using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IGenericRepo<Supplier> _supplierRepo;

        public SupplierController(IGenericRepo<Supplier> repo)
        {
            _supplierRepo = repo;
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSupplier(SupplierViewModel supplier) 
        {
            if(ModelState.IsValid) 
            {
                Supplier supplierModel = new Supplier()
                {
                    Name = supplier.Name,
                    Address = supplier.Address,
                    ContactInfo = supplier.ContactInfo,
                };

                _supplierRepo.AddAsync(supplierModel);
            }

            return View();
        }

        public IActionResult SupplierList()
        {
            var suppliers = _supplierRepo.GetAll();

            return View(suppliers);
        }

        public IActionResult EditSupplier(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            var supplier = _supplierRepo.GetById((int)id);

            if(supplier is null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        public IActionResult EditSupplier(Supplier supplieModel)
        {
            if (ModelState.IsValid) 
            {
                _supplierRepo.Update(supplieModel);
                return RedirectToAction("SupplierList");
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeleteSupplier(int? id)
        {
            if(id is null || id == 0)
                return NotFound();

            var supplier = _supplierRepo.GetById((int)id);

            if(supplier is null)
                return NotFound();

            _supplierRepo.Delete(supplier);
            return RedirectToAction("SupplierList");
        }
    }

}
