using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.DataAcess.Data;
using Bookstore.Models;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BookstoreWeb.Areas.Admin.Controllers
{
   [Area("Admin")]
   public class ProductController : Controller
   {
      private readonly IUnitOfWork _unitOfWork;
      public ProductController(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      public IActionResult Index()
      {
         List<Product> products = _unitOfWork.Product.GetAll().ToList();
         IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
         {
            Text = u.Name,
            Value = u.Id.ToString()
         });
         return View(products);
      }

      public IActionResult Upsert(int? id)
      {
         ProductVM productVM = new()
         {
            CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
               Text = u.Name,
               Value = u.Id.ToString()
            }),
            Product = new Product(),
         };

         if(id == null || id == 0) // create
         {
            return View(productVM);
         } 
         else // update
         {
            productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
            return View(productVM);
         }
      }

      [HttpPost]
      public IActionResult Create(ProductVM productVM, IFormFile? file)
      {
         if (ModelState.IsValid)
         {
            _unitOfWork.Product.Add(productVM.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created!";
            return RedirectToAction("Index");
         } else
         {
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
               Text = u.Name,
               Value = u.Id.ToString()
            });
            return View(productVM);
         }
      }

      public IActionResult Delete(int? id)
      {
         if (id == null || id == 0)
         {
            return NotFound();
         }

         Product? Product = _unitOfWork.Product.Get(p => p.Id == id);

         if (Product == null)
         {
            return NotFound();
         }
         return View(Product);
      }

      [HttpPost, ActionName("Delete")]
      public IActionResult DeletePOST(int? id)
      {
         Product obj = _unitOfWork.Product.Get(p => p.Id == id);
         _unitOfWork.Product.Delete(obj);
         _unitOfWork.Save();
         TempData["success"] = "Product deleted!";
         return RedirectToAction("Index");
      }
   }
}
