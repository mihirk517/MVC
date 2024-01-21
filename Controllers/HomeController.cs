using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.DataAccess.Repository;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitofWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitofWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitofWork.ProductRepository.GetAll(includeprops: nameof(Category));
            return View(products);
        }

        /// <summary>
        /// Details View Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitofWork.ProductRepository.Get(u => u.Id == id, includeprops: nameof(Category)),
                Count = 1,
                ProductId = id
            };
            Debug.WriteLine($" Id {shoppingCart.Id}");
            //Product product = _unitofWork.ProductRepository.Get(u => u.Id == id,includeprops:nameof(Category));
            return View(shoppingCart);
        }
        /// <summary>
        /// Details Post Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            try
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                Debug.WriteLine($"User Id {userId}");
                shoppingCart.UserId = userId;

                ShoppingCart cart = _unitofWork.ShoppingCartRepository.Get(x => x.UserId == userId && x.ProductId == shoppingCart.ProductId);
                if (cart != null)
                {
                    cart.Count = shoppingCart.Count;
                    _unitofWork.ShoppingCartRepository.Update(cart);
                }
                else
                {   if (shoppingCart.Id != 0)
                    {
                        Debug.WriteLine($"Shopping Cart Id exists {shoppingCart.Id}");
                        shoppingCart.Id = 0;
                    }
                    _unitofWork.ShoppingCartRepository.Add(shoppingCart);
                }          
                _unitofWork.Save();

            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.Message}\n {e.Source}");
                //throw;
            }

            return RedirectToAction(nameof(Index));
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
