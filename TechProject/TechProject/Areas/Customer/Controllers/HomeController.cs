using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechProject.Models;
using TechProject.DataAccess;
using TechProject.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
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
        public IActionResult Index()
        {
            var ProductList = _unitofwork.Product
                .GetAll(null, "Category,ProductImages") 
                .ToList();

            return View(ProductList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
