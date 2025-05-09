using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadMe.DataAccess.Repository.IRepository;
using ReadMe.Models.Models;
using ReadMe.Utility;

namespace ReadMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWorkcs _unit;
        public CategoryController(IUnitOfWorkcs unit)
        {
            _unit = unit;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var categories = _unit.categoryRepository.GetAll().ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unit.categoryRepository.Add(category);
                _unit.Save();
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");


            }
            return View();


        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = _unit.categoryRepository.Get(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {

                _unit.categoryRepository.Update(category);
                _unit.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unit.categoryRepository.Get(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            if (category is null) { return NotFound(); }
            _unit.categoryRepository.Remove(category);
            _unit.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");





        }

    }
}
