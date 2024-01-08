using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using System.IO;
using MVC.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using MVC.DataAccess.Utilities;

namespace MVC.Controllers
{
    [Authorize(Roles =Roles.Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> data = _unitOfWork.ProductRepository.GetAll(includeprops: nameof(Category)).ToList();

            return View(data);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? Item = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            //Product? Item = _databaseContext.products.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return NotFound();
            }
            return View(Item);
        }
        [HttpPost]
        public IActionResult Edit(Product data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(data);
                _unitOfWork.Save();
                //_databaseContext.Update(data);
                //_databaseContext.SaveChanges();
                TempData["success"] = $"Category {data.Title} edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? Item = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            //Category? Item = _databaseContext.categories.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return NotFound();
            }
            return View(Item);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? Item = _unitOfWork.ProductRepository.Get(x => x.Id == id);
            //Category? Item = _databaseContext.categories.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepository.Remove(Item);
            _unitOfWork.Save();
            //_databaseContext.Remove(Item);
            //_databaseContext.SaveChanges();
            TempData["success"] = $"Category {Item.Title} deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM data, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    System.Diagnostics.Debug.WriteLine(wwwRootPath);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(data.Product.ImageUrl))
                    {
                        Console.WriteLine($"Image exists{data.Product.ImageUrl}");
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, data.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    data.Product.ImageUrl = @"images\product\" + fileName;
                }
                if (data.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(data.Product);
                    TempData["success"] = $"Category {data.Product.Title} created successfully";
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(data.Product);
                    TempData["success"] = $"Category {data.Product.Title} updated successfully";
                }
                _unitOfWork.Save();
                //_databaseContext.AddAsync(data);
                //_databaseContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                data.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(U => new SelectListItem
                {
                    Text = U.Name,
                    Value = U.Id.ToString()
                });
                return View(data);
            }

        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeprops: nameof(Category)).ToList();
            return Json(new { data = products });
        }
        #endregion
    }
}
