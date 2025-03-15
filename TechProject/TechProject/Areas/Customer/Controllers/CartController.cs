using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.ViewModels;

namespace TechProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController:Controller
    {
        private IUnitOfWork _unitofOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitofOfWork = unitOfWork;
        }
        public IActionResult Plus(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s=>s.Id==cartId);
            cartfromdb.count +=1  ;
            _unitofOfWork.ShoppingCart.Update(cartfromdb);
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s => s.Id == cartId);
            if (cartfromdb.count <= 1)
            {
                _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            }
            else
            {
                cartfromdb.count -= 1;
                _unitofOfWork.ShoppingCart.Update(cartfromdb);
            }
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s => s.Id == cartId);
            _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
       
        public IActionResult Index()
        { 
            var claimsIdentity =(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product")
            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.totalprice += (double)(cart.Product.Price*cart.count);
            }
         return View(ShoppingCartVM);
        }
        public IActionResult SummaryOrder()
        {
            return View();
        }
    }
}
