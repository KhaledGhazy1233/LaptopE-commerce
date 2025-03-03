using Microsoft.AspNetCore.Mvc;
using TechProject.DataAccess.Data;
using Tech.DataAccess.Repository.IRepository;
using TechProject.Models;
using Tech.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Tech.Utility;

namespace TechProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=SD.Role_User_Admin)]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var categories = _unitofwork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
             TempData["success"] = "CategoryCreateSuccessfully";

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name.Any(char.IsDigit))
            {
                ModelState.AddModelError("Name", "cant access Number To Name");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(category);
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        //Edit
        public IActionResult Edit(int id)
        {
            TempData["success"] = "CategoryUpdateSuccessfully";
            if (id == 0)
                return NotFound();
            var category = _unitofwork.Category.Get(c => c.Id == id);
            if (category == null)
                return NotFound();
            return View(category);
        }
        //Edit
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(category);
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            TempData["success"] = "CategoryDeleteSuccessfully";
            if (id == 0)
                return NotFound();
            var category = _unitofwork.Category.Get(u => u.Id == id);
            if (category == null)
                return NotFound();
            return View(category);
        }
        //Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteRow(int id)
        {
            var category = _unitofwork.Category.Get(c => c.Id == id);
            if (category == null)
                return NotFound();
            _unitofwork.Category.Remove(category);
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
    }
}
