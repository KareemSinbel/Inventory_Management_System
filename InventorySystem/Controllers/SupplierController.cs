using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierRepo repo;

        public SupplierController(SupplierRepo repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            var result = repo.GetAll();
            return View(result);
        }
        public IActionResult Details(int id)
        {
            var result = repo.GetById(id);
            return View(result);
        }
        public IActionResult Delete(int id)
        {
            repo.DeleteById(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Supplier obj)
        {
            repo.Update(obj);
            return RedirectToAction("Index");
        }

    }
    
}
