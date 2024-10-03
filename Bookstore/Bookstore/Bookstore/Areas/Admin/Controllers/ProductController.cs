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
      private readonly IWebHostEnvironment _webHostEnvironment;
      public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
      {
         _unitOfWork = unitOfWork;
         _webHostEnvironment = webHostEnvironment;
      }

      public IActionResult Index()
      {
         List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
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

         if (id == null || id == 0) // create
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
      public IActionResult Upsert(ProductVM productVM, IFormFile? file)
      {
         if (ModelState.IsValid)
         {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
               string fileName = Guid.NewGuid().ToString();
               string filePath = Path.Combine(wwwRootPath, @"images\products", fileName + Path.GetExtension(file.FileName));

               if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
               {
                  // delete old image
                  var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                  if (System.IO.File.Exists(oldImagePath))
                  {
                     System.IO.File.Delete(oldImagePath);
                  }
               }

               using (var fileStream = new FileStream(filePath, FileMode.Create))
               {
                  file.CopyTo(fileStream);
               }

               productVM.Product.ImageUrl = @"\images\products\" + fileName + Path.GetExtension(file.FileName);
            }

            // add functionality
            if (productVM.Product.Id == 0)
            {
               _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
               _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
            TempData["success"] = "Product created!";
            return RedirectToAction("Index");
         }
         else
         {
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
               Text = u.Name,
               Value = u.Id.ToString()
            });
            return View(productVM);
         }
      }

      //public IActionResult Delete(int? id)
      //{
      //   if (id == null || id == 0)
      //   {
      //      return NotFound();
      //   }

      //   Product? Product = _unitOfWork.Product.Get(p => p.Id == id);

      //   if (Product == null)
      //   {
      //      return NotFound();
      //   }
      //   return View(Product);
      //}

      //[HttpPost, ActionName("Delete")]
      //public IActionResult DeletePOST(int? id)
      //{
      //   Product obj = _unitOfWork.Product.Get(p => p.Id == id);
      //   _unitOfWork.Product.Delete(obj);
      //   _unitOfWork.Save();
      //   TempData["success"] = "Product deleted!";
      //   return RedirectToAction("Index");
      //}

      #region APIs
      [HttpGet]
      public IActionResult GetAll()
      {
         List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
         return Json(new { data = products });
      }

      [HttpDelete]
      public IActionResult Delete(int? id)
      {
         Product deletingProduct = _unitOfWork.Product.Get(p => p.Id == id);
         if (deletingProduct == null)
         {
            return Json(new { success = false, message = "Error deleting product" });
         }
         var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, deletingProduct.ImageUrl.TrimStart('\\'));

         if (System.IO.File.Exists(oldImagePath))
         {
            System.IO.File.Delete(oldImagePath);
         }
         _unitOfWork.Product.Delete(deletingProduct);
         _unitOfWork.Save();
         return Json(new { success = true, message = "Product deleted" });
      }
      #endregion
   }
}
