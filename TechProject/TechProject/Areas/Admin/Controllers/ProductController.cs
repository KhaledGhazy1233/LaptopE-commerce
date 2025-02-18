using Microsoft.AspNetCore.Mvc;
using TechProject.DataAccess.Data;
using Tech.DataAccess.Repository.IRepository;
using TechProject.Models;
using Tech.DataAccess.Repository;

namespace TechProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofwork;
        public ProductController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var products = _unitofwork.Product.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            // TempData["success"] = "ProductCreateSuccessfully";
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            
            if (ModelState.IsValid)
            {
                _unitofwork.Product.Add(product);
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        //Edit
        public IActionResult Edit(int id)
        {
            TempData["success"] = "ProductUpdateSuccessfully";
            if (id == 0)
                return NotFound();
            var product = _unitofwork.Product.Get(c => c.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        //Edit
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.Product.Update(product);
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            TempData["success"] = "ProductDeleteSuccessfully";
            if (id == 0)
                return NotFound();
            var product = _unitofwork.Product.Get(u => u.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }
        //Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteRow(int id)
        {
            var product = _unitofwork.Product.Get(c => c.Id == id);
            if (product == null)
                return NotFound();
            _unitofwork.Product.Remove(product);
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
    }
}
