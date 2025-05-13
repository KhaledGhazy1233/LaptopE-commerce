using Microsoft.AspNetCore.Mvc;
using TechProject.DataAccess.Data;
using Tech.DataAccess.Repository.IRepository;
using TechProject.Models;
using Tech.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Tech.Utility;
using Tech.Models.Models;

namespace TechProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
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
        public IActionResult Index(int page = 1)
        {
            int pageSize = 5; 
            var products = _db.Products.Include(p => p.Category).ToList();
            var categories = _db.Categories.ToList();

            ViewData["Categories"] = categories;

            int totalPages = (int)Math.Ceiling((double)products.Count() / pageSize);

            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedProducts);
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
                    }),
                ProductImages = new List<ProductImage>()
            };

            if (id != 0)
            {
                productVM.product = _unitofwork.Product
           .GetAll(p => p.Id == id, "ProductImages")
           .FirstOrDefault();
                if (productVM.product == null)
                {
                    return NotFound();
                }
                productVM.ProductImages = productVM.product.ProductImages.ToList() ?? new List<ProductImage>();
            }

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM)
        {
            var files = productVM.Files;
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

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string productPath = Path.Combine(wwwRootPath, "images", "product");

            if (!Directory.Exists(productPath))
            {
                Directory.CreateDirectory(productPath);
            }

            bool isNewProduct = productVM.product.Id == 0;

            if (isNewProduct)
            {
                _unitofwork.Product.Add(productVM.product);
                _unitofwork.Save();
            }
            else
            {
                _unitofwork.Product.Update(productVM.product);
            }

            var existingImages = _unitofwork.Product
                .GetAll(p => p.Id == productVM.product.Id, "ProductImages")
                .FirstOrDefault()?
                .ProductImages
                .Select(img => img.ImageUrl)
                .ToList() ?? new List<string>();

            List<ProductImage> newImages = new List<ProductImage>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(productPath, fileName);
                    string relativePath = @"\images\product\" + fileName;

                    if (!existingImages.Contains(relativePath))
                    {
                        using (var filestream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(filestream);
                        }

                        newImages.Add(new ProductImage()
                        {
                            ImageUrl = relativePath,
                            ProductId = productVM.product.Id
                        });
                    }
                }

                if (newImages.Count > 0)
                {
                    _unitofwork.Product.AddProductImages(productVM.product.Id, newImages);
                }
            }

           
            var productImages = _unitofwork.Product
                .GetAll(p => p.Id == productVM.product.Id, "ProductImages")
                .FirstOrDefault()?.ProductImages
                .Select(img => img.ImageUrl)
                .ToList();

            if (string.IsNullOrEmpty(productVM.product.headerImageUrl) && productImages.Count > 0)
            {
                    productVM.product.headerImageUrl = productImages.First();
               
            }

            _unitofwork.Save();
            return RedirectToAction("Index");
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
            else
            {
                var deletedProduct = _unitofwork.Product.DeleteProduct(id);
                if (deletedProduct == true) return Json(new { success = true, message = "Product deleted successfully!" });
                else return Json(new { success = false, message = "Product not found!" });
            }
           
            //var product = _unitofwork.Product.Get(u => u.Id == id);
            //if (product == null)
            //    return Json(new { success = false, message = "Product not found!" });
            //if (!(product.ImageUrl == null))
            //{
            //    var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.Trim('\\'));
            //    if (System.IO.File.Exists(OldImagePath))
            //    {
            //        System.IO.File.Delete(OldImagePath);
            //    }
            //}


            //// Remove images from the database
            //_db.ProductImages.RemoveRange(product.ProductImages);

            //_unitofwork.Product.Remove(product);
            //_unitofwork.Save();

            //return Json(new { success = true, message = "Product deleted successfully!" });

        }
        [HttpPost]
        [Route("Admin/Product/DeleteImage/{id}")]
        public IActionResult DeleteImage(int id)
        {
            var image = _db.ProductImages.Find(id);
            if (image == null)
                return NotFound(new { success = false, message = "Image not found" });

            bool isDeleted = _unitofwork.Product.DeleteImages(new List<ProductImage> { image });
            if (!isDeleted)
                return BadRequest(new { success = false, message = "Failed to delete" });

            return Ok(new { success = true, message = "Deleted successfully" });
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
