using Microsoft.AspNetCore.Mvc;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using MVC.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            List<Category> data = _categoryRepo.GetAll().ToList();
            //List<Category> data = _databaseContext.categories.ToList();
            return View(data);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? Item = _categoryRepo.Get(x => x.Id == id);
            //Category? Item = _databaseContext.categories.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return NotFound();
            }
            return View(Item);
        }
        [HttpPost]
        public IActionResult Edit(Category data)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(data); 
                _categoryRepo.Save();
                //_databaseContext.Update(data);
                //_databaseContext.SaveChanges();
                TempData["success"] = $"Category {data.Name} edited successfully";
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
            Category? Item = _categoryRepo.Get(x => x.Id == id);
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
            Category? Item = _categoryRepo.Get(x => x.Id == id);
            //Category? Item = _databaseContext.categories.FirstOrDefault(x => x.Id == id);
            if (Item == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(Item);
            _categoryRepo.Save();
            //_databaseContext.Remove(Item);
            //_databaseContext.SaveChanges();
            TempData["success"] = $"Category {Item.Name} deleted successfully";
            return RedirectToAction("Index");

        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category data)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(data);
                _categoryRepo.Save();
                //_databaseContext.AddAsync(data);
                //_databaseContext.SaveChanges();
                TempData["success"] = $"Category {data.Name} created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }
    }
}
