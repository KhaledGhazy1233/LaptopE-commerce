using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using static System.Net.Mime.MediaTypeNames;
using TechProject.Models;
using Tech.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Tech.Utility;

namespace TechProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class DiscountAndOffersController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public DiscountAndOffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var Discounts = _unitOfWork.Discount.GetAll(includeProperties: "Product");
            return View(Discounts);
        }
        [HttpGet]
        public IActionResult AddDiscount()
        {
            
            var products = _unitOfWork.Product.GetAll();
           
            var discountedProductIds = _unitOfWork.Discount.GetAll()
     .Select(d => d.ProductId)
     .ToHashSet(); 
            var productList = products
                .Where(p => !discountedProductIds.Contains(p.Id)) 
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
                .ToList();
            var discountVM = new DiscountVM
            {
                products = productList,
                discount = new Discount()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }
            };
            return View(discountVM);
        }
        [HttpPost]
        public IActionResult AddDiscount(DiscountVM discountVM)
        {
            ModelState.Remove("products");
            if (ModelState.IsValid)
            {
                if (discountVM.discount.EndDate <= discountVM.discount.StartDate)
                {
                    ModelState.AddModelError("discount.EndDate", "End Date must be greater than Start Date.");
                }
                var discount = discountVM.discount;

                discount.Presentage = discount.Presentage / 100;

                _unitOfWork.Discount.Add(discount);
                _unitOfWork.Discount.Add(discount);
                _unitOfWork.Save();
                return Redirect("index");
            }
            else
            {
                var products = _unitOfWork.Product.GetAll();
                var discountedProductIds = _unitOfWork.Discount.GetAll()
     .Select(d => d.ProductId)
     .ToHashSet(); 

                var productList = products
                    .Where(p => !discountedProductIds.Contains(p.Id)) 
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToList();

                return View(discountVM);
            }

        }
        [HttpGet]
        public IActionResult UpdateDiscount(int id)
        {
            var discount = _unitOfWork.Discount.GetAll(d => d.Id == id, includeProperties: "Product").FirstOrDefault();
            discount.Presentage *=100;
            if (discount == null)
            {
                return NotFound();
            }

           
            return View(discount);
        }
        [HttpPost]
        public IActionResult UpdateDiscount(Discount discount)
        {
            ModelState.Remove("products");
            if (ModelState.IsValid)
            {
                
                discount.Presentage = discount.Presentage / 100;
                _unitOfWork.Discount.Update(discount);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        public JsonResult DeleteDiscount(int id)
        {
            var discount = _unitOfWork.Discount.Get(d => d.Id == id);
            if (discount == null)
            {
                return Json(new { success = false, message = "Discount not found." });
            }

            _unitOfWork.Discount.Remove(discount);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Discount deleted successfully." });
        }


    }
}
