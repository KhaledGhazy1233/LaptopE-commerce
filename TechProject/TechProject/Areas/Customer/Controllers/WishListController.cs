using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tech.DataAccess.Repository.IRepository;
using Tech.Models.Models;
using TechProject.Models;

namespace TechProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult AddToWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var wishlist = _unitOfWork.WishList.Get(w => w.ApplicationUserId == userId);
            if (wishlist == null)
            {
                wishlist = new WishList
                {
                    ApplicationUserId = userId
                };
                _unitOfWork.WishList.Add(wishlist);
                _unitOfWork.Save();
            }

            var existingItem = _unitOfWork.WishListItem.Get(wi => wi.WishlistId == wishlist.Id && wi.ProductId == productId);
            if (existingItem == null)
            {
                var newItem = new WishListItem
                {
                    WishlistId = wishlist.Id,
                    ProductId = productId
                };
                _unitOfWork.WishListItem.Add(newItem);
                _unitOfWork.Save();
            }

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlist = _unitOfWork.WishList.Get(w => w.ApplicationUserId == userId);

            if (wishlist == null)
            {
                return View(new List<WishListItem>());
            }

            var wishlistItems = _unitOfWork.WishListItem.GetAll(
                wi => wi.WishlistId == wishlist.Id,
                includeProperties: "Product"
            );

            return View(wishlistItems);
        }

        public IActionResult RemoveFromWishlist(int wishlistItemId)
        {
            var wishlistItem = _unitOfWork.WishListItem.Get(wi => wi.Id == wishlistItemId);
            if (wishlistItem != null)
            {
                _unitOfWork.WishListItem.Remove(wishlistItem);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public int GetWishlistCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return 0;

            var wishlist = _unitOfWork.WishList.Get(w => w.ApplicationUserId == userId);
            if (wishlist == null)
                return 0;

            return _unitOfWork.WishListItem.GetAll(wi => wi.WishlistId == wishlist.Id).Count();
        }
    }
}
