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
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitofwork, ApplicationDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
            ProductVM productVM = new()
            {
                product = new Product(),
                CategoryList = _unitofwork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    })
            };

            if (id != 0)
            {
                productVM.product = _unitofwork.Product.Get(p => p.Id == id);
                if (productVM.product == null)
                {
                    return NotFound();
                }
            }

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                productVM.CategoryList = _unitofwork.Category.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                    });

                return View(productVM);
            }
            else
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productpath = Path.Combine(wwwRootPath, "images", "product");
                    if (!string.IsNullOrEmpty(productVM.product.ImageUrl))
                    {
                        var OldImagePath = Path.Combine(wwwRootPath, productVM.product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }

                    }
                    else
                    {
                        using (var filestream = new FileStream(Path.Combine(productpath, fileName), FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }
                    }

                    productVM.product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.product.Id == 0)
                {
                    _unitofwork.Product.Add(productVM.product);
                }
                else
                {
                    _unitofwork.Product.Update(productVM.product);
                }

                _unitofwork.Save();
                return RedirectToAction("Index");
            }
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
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return Json(new { success = false, message = "Invalid Product ID!" });

            var product = _unitofwork.Product.Get(u => u.Id == id);
            if (product == null)
                return Json(new { success = false, message = "Product not found!" });
            
                var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.Trim('\\'));
                if (System.IO.File.Exists(OldImagePath))
                {
                    System.IO.File.Delete(OldImagePath);
                }
            _unitofwork.Product.Remove(product);
            _unitofwork.Save();

            return Json(new { success = true, message = "Product deleted successfully!" });
        }

        #region Apis
        public IActionResult GetAll()
        { 
            var products = _unitofwork.Product.GetAll().ToList();
            return Json( new {data = products });
        }
        #endregion
    }
}
