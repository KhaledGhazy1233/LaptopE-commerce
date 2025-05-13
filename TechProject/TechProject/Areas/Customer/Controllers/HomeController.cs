using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechProject.Models;
using TechProject.DataAccess;
using TechProject.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.ViewModels;
namespace TechProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private IUnitOfWork _unitofwork;
        public HomeController(ILogger<HomeController> logger ,ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitofwork = unitOfWork;
        }

        public IActionResult Details(int id)
        {
            var product = _unitofwork.Product
                .GetAll(p => p.Id == id, "ProductImages,Category")
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);



        }
        public IActionResult Index(string searchTerm ,string category)
        {
            var ProductList = _unitofwork.Product
             .GetAll(null, "Category,ProductImages")
             .ToList();
           
            if (!string.IsNullOrEmpty(searchTerm))
            {
                ProductList = ProductList.Where(u => u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }
            foreach (var product in ProductList)
            {
                var discount = _unitofwork.Discount.Get(d => d.ProductId == product.Id); 
                if (discount != null && discount.IsActive)
                {
                    product.Presentage = discount.Presentage * 100;
                    product.PriceAfterDiscount = Math.Round((double)product.Price * (1 - discount.Presentage), 2); 
                }
                else
                {
                    product.PriceAfterDiscount = (double)product.Price; 
                }
            }
            ViewData["ActivePage"] = "Home";
            
            

            return View(ProductList);
        }



        public IActionResult Privacy()
        {
            ViewData["ActivePage"] = "Privacy";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
