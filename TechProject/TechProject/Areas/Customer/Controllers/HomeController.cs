using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechProject.Models;
using TechProject.DataAccess;
using TechProject.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace TechProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private IUnitOfWork _unitofwork;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitofwork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Details(int id)
        {

            ShoppingCart cart = new()
            {
                ProductId = id,
                count = 1,
             Product = _unitofwork.Product.GetAll(p => p.Id == id, includeProperties: "Category").SingleOrDefault()
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            var cartfromDb = _unitofwork.ShoppingCart.Get(sc =>sc.ApplicationUserId == userId && sc.ProductId == shoppingCart.ProductId);
            if (cartfromDb != null)
            {
                cartfromDb.count += shoppingCart.count;
                _unitofwork.ShoppingCart.Update(cartfromDb);

            }
            else
            {
                _unitofwork.ShoppingCart.Add(shoppingCart);
            }
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var ProductList = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();
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
