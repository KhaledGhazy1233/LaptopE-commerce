using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace TechProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
             var cart = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();
            return View(cart);
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            if (productId==0&& quantity==0)
            {
                return RedirectToAction("index","home");
            }
            var userId = _userManager.GetUserId(User);
            var cartItem = _unitOfWork.Cart.Get(c => c.UserId == userId && c.ProductId == productId);
            var productPrice = _unitOfWork.Product.Get(c => c.Id == productId).Price;

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                cartItem.TotalPrice = productPrice * cartItem.Quantity;
                _unitOfWork.Cart.Update(cartItem);
            }
            else
            {
                _unitOfWork.Cart.Add(new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    TotalPrice = productPrice * quantity
                });
            }

            _unitOfWork.Save();
            return RedirectToAction("details", "home", new { id = productId });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartId, int quantity)
        {
            var cartItem = _unitOfWork.Cart.Get(c => c.Id == cartId);
            var productPrice = _unitOfWork.Product.Get(c => c.Id == cartItem.ProductId).Price;
            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                cartItem.TotalPrice = productPrice * cartItem.Quantity;
                _unitOfWork.Cart.Update(cartItem);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int cartId)
        {
            var cartItem = _unitOfWork.Cart.Get(c => c.Id == cartId);
            if (cartItem != null)
            {
                _unitOfWork.Cart.Remove(cartItem);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }
        public IActionResult OrderConfirmation(List<Cart> cartItems)
        {
           

            return View(cartItems);
        }
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _unitOfWork.Cart.GetAll(c => c.UserId == userId, includeProperties: "Product").ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }
            ViewBag.totalPrice = cartItems.Sum(item => item.TotalPrice);
            // TODO: Process payment & create an order
            return View("OrderConfirmation", cartItems);
        }

        public IActionResult GetCartCount()
        {
            var userId = _userManager.GetUserId(User);
            var cartCount = _unitOfWork.Cart.GetAll().Where(c => c.UserId == userId).Sum(c => c.Quantity);
            return Json(cartCount);
        }
    }
}
