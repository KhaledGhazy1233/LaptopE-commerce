using Microsoft.AspNetCore.Mvc;
using TechProject.DataAccess.Data;
using Tech.DataAccess.Repository.IRepository;
using TechProject.Models;
using Tech.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tech.Models.ViewModels;

namespace TechProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IUnitOfWork _unitofwork;
        public ProductController(IUnitOfWork unitofwork, ApplicationDbContext db)
        {
            _unitofwork = unitofwork;
            _db = db;
        }
        public IActionResult Index()
        {
            var products = _db.Products.Include(p => p.Category).ToList();
            return View(products); 
        }

        public IActionResult IndexList()
        {
            var products = _unitofwork.Product.GetAll().ToList();
            return View(products);
        }

        public IActionResult Upsert(int id)
        {

            IEnumerable<SelectListItem> CategoryList = _unitofwork.Category.GetAll()
                .Select(c=>new SelectListItem 
                { 
                 Text = c.Name,
                 Value= c.Id.ToString(),
                });
            ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
                product = new Product(),
                CategoryList = CategoryList
            };

            if (id == 0)
            {
                // create
                return View(productVM);
            }
            else
            {
                productVM.product = _unitofwork.Product.Get(p=>p.Id==id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile ? file)
        {
            
            if (ModelState.IsValid)
            {
                if (productVM.product.Id == 0)
                {
                    _unitofwork.Product.Add(productVM.product);
                }
                else
                {
                    _unitofwork.Product.Update(productVM.product);
                }
                _unitofwork.Save();
            }
            return View();
        }
        #region
        ////Edit
        //public IActionResult Edit(int id)
        //{
        //    TempData["success"] = "ProductUpdateSuccessfully";
        //    if (id == 0)
        //        return NotFound();
        //    var product = _unitofwork.Product.Get(c => c.Id == id);
        //    if (product == null)
        //        return NotFound();
        //    return View(product);
        //}
        ////Edit
        //[HttpPost]
        //public IActionResult Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.Product.Update(product);
        //        _unitofwork.Save();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        #endregion
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
