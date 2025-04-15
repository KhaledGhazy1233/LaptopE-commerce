using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tech.DataAccess.Repository;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.ViewModels;
using TechProject.Models;

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
        public IActionResult AddToCart(int productId, int quantity)
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItem = _unitofOfWork.ShoppingCart.Get(c => c.ApplicationUserId == userId && c.ProductId == productId);
           

            if (cartItem != null)
            {
                cartItem.count += quantity;
                
                _unitofOfWork.ShoppingCart.Update(cartItem);
            }
            else
            {
                _unitofOfWork.ShoppingCart.Add(new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    count = quantity,
                   
                });
            }

            _unitofOfWork.Save();
            return RedirectToAction("details", "home", new { id = productId });
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
        [AllowAnonymous]
        public int GetCartCount()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return 0; 
            }else
            return _unitofOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId).Sum(c => c.count);
        }

    }
}
