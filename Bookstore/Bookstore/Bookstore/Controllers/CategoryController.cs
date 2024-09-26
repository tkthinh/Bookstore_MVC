using Microsoft.AspNetCore.Mvc;
using Bookstore.Models;
using Bookstore.Data;

namespace Bookstore.Controllers
{
   public class CategoryController : Controller
   {
      private readonly ApplicationDbContext _db;
      public CategoryController(ApplicationDbContext db)
      {
         _db = db;
      }

      public IActionResult Index()
      {
         var categories = _db.Categories.ToList();
         return View(categories);
      }

      public IActionResult Create()
      {
         return View();
      }

      [HttpPost]
      public IActionResult Create(Category obj)
      {
         if (ModelState.IsValid)
         {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Category created!";
            return RedirectToAction("Index");
         }
         return View(obj);
      }

      public IActionResult Edit(int? id)
      {
         if(id ==null || id == 0)
         {
            return NotFound();
         }

         Category category = _db.Categories.FirstOrDefault(c => c.Id == id);
         if (category == null)
         {
            return NotFound();
         }
         return View(category);
      }

      [HttpPost]
      public IActionResult Edit(Category obj)
      {
         if (ModelState.IsValid)
         {
            _db.Categories.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Category updated!";
            return RedirectToAction("Index");
         }
         return View(obj);
      }

      public IActionResult Delete(int? id)
      {
         if (id == null || id == 0)
         {
            return NotFound();
         }

         Category? category = _db.Categories.FirstOrDefault(obj => obj.Id == id);

         if (category == null)
         {
            return NotFound();
         }
         return View(category);
      }

      [HttpPost, ActionName("Delete")]
      public IActionResult DeletePOST(int? id)
      {
         Category obj = _db.Categories.FirstOrDefault(obj => obj.Id == id);
         _db.Categories.Remove(obj);
         _db.SaveChanges();
         TempData["success"] = "Category deleted!";
         return RedirectToAction("Index");
      }
   }
}
