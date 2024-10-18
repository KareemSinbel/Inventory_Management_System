using InventorySystem.Models;
using InventorySystem.Repositories;
using InventorySystem.ViewModels;
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
        public IActionResult EditSupplier()
        {
            return View();
        }


        //public IActionResult Index()
        //{
        //    var result = repo.GetAll();
        //    return View(result);
        //}
        //public IActionResult Details(int id)
        //{
        //    var result = repo.GetById(id);
        //    return View(result);
        //}
        //public IActionResult Delete(int id)
        //{
        //    repo.DeleteById(id);
        //    return RedirectToAction("Index");
        //}
        //public IActionResult Edit()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Edit(Supplier obj)
        //{
        //    repo.Update(obj);
        //    return RedirectToAction("Index");
        //}

    }
    
}
