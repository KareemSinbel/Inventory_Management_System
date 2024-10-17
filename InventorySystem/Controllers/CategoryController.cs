using InventorySystem.Models;
using InventorySystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
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
