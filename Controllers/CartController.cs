using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using MVC.Models.ViewModels;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll((x => x.UserId == userId),
                includeprops: nameof(Product)),
                orderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVM.shoppingCartList)
            {
                cart.Price = GetOrderTotal(cart);
                ShoppingCartVM.orderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);
            cart.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cart);
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cart);

            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);

            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll((x => x.UserId == userId),
                includeprops: nameof(Product)),
                orderHeader = new OrderHeader()
            };

            //_unitOfWork.OrderHeaderRepository.Add(orderHeader);
            foreach (var cart in ShoppingCartVM.shoppingCartList)
            {
                cart.Price = GetOrderTotal(cart);
                ShoppingCartVM.orderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        public IActionResult Summary(ShoppingCartVM shoppingCart)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartVM.shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll((x => x.UserId == userId),
            includeprops: nameof(Product));

            ShoppingCartVM.orderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.orderHeader.UserId = userId;

            ShoppingCartVM.orderHeader.Name = shoppingCart.orderHeader.Name;
            ShoppingCartVM.orderHeader.StreetAddress = shoppingCart.orderHeader.StreetAddress;
            ShoppingCartVM.orderHeader.PhoneNumber = shoppingCart.orderHeader.PhoneNumber;
            ShoppingCartVM.orderHeader.City = shoppingCart.orderHeader.City;
            ShoppingCartVM.orderHeader.State = shoppingCart.orderHeader.State;
            ShoppingCartVM.orderHeader.PostalCode = shoppingCart.orderHeader.PostalCode;

            foreach (var cart in ShoppingCartVM.shoppingCartList)
            {
                cart.Price = GetOrderTotal(cart);
                ShoppingCartVM.orderHeader.OrderTotal += cart.Price * cart.Count;
                
            }            
            _unitOfWork.OrderHeaderRepository.Add(ShoppingCartVM.orderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVM.shoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.orderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetailRepository.Add(orderDetail);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(OrderConfirmation),new { id = ShoppingCartVM.orderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.Get(u => u.Id == id);
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository.GetAll(x => x.UserId == orderHeader.UserId).ToList();
            _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View();
        }
        private double GetOrderTotal(ShoppingCart shoppingCart)
        {
            switch (shoppingCart.Count)
            {
                case <= 50:
                    return shoppingCart.Product.Price;
                case >= 50 and <= 100:
                    return shoppingCart.Product.Price50;
                case >= 100:
                    return shoppingCart.Product.Price100;
            }
        }
    }
}
