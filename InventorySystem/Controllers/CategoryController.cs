using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {

        private readonly IGenericRepo<Category> _categoryRepo;

        public CategoryController(IGenericRepo<Category> repo)
        {
            _categoryRepo = repo;
        }

        public IActionResult CategoryList()
        {
            var categories = _categoryRepo.GetAll();
            return View(categories);
        }
        public IActionResult EditCategory()
        {
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _categoryRepo.AddAsync(category);
            }
            return View();
        }
    }
}
