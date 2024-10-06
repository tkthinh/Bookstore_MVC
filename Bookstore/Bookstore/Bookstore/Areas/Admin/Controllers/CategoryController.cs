using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.DataAcess.Data;
using Bookstore.Models;
using Bookstore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookstoreWeb.Areas.Admin.Controllers
{
   [Area("Admin")]
   [Authorize(Roles = SD.Role_Admin)]
   public class CategoryController : Controller
   {
      private readonly IUnitOfWork _unitOfWork;
      public CategoryController(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public IActionResult Index()
      {
         var categories = _unitOfWork.Category.GetAll().ToList();
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
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created!";
            return RedirectToAction("Index");
         }
         return View(obj);
      }

      public IActionResult Edit(int? id)
      {
         if (id == null || id == 0)
         {
            return NotFound();
         }

         Category category = _unitOfWork.Category.Get(c => c.Id == id);
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
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
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

         Category? category = _unitOfWork.Category.Get(c => c.Id == id);

         if (category == null)
         {
            return NotFound();
         }
         return View(category);
      }

      [HttpPost, ActionName("Delete")]
      public IActionResult DeletePOST(int? id)
      {
         Category obj = _unitOfWork.Category.Get(c => c.Id == id);
         _unitOfWork.Category.Delete(obj);
         _unitOfWork.Save();
         TempData["success"] = "Category deleted!";
         return RedirectToAction("Index");
      }
   }
}
