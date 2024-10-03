using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.DataAcess.Data;
using Bookstore.Models;

namespace Bookstore.DataAccess.Repository
{
   public class ProductRepository : Repository<Product>, IProductRepository
   {
      private ApplicationDbContext _db;
      public ProductRepository(ApplicationDbContext db) : base(db)
      {
         _db = db;
      }

      public void Update(Product product)
      {
         var existingProduct = _db.Products.FirstOrDefault(p => p.Id == product.Id);
         if(existingProduct != null)
         {
            existingProduct.Id = product.Id;
            existingProduct.ISBN = product.ISBN;
            existingProduct.ListPrice = product.ListPrice;
            existingProduct.Price = product.Price;
            existingProduct.Price50 = product.Price50;
            existingProduct.Price100 = product.Price100;
            existingProduct.Description = product.Description;
            existingProduct.Author = product.Author;
            if (existingProduct.ImageUrl != null)
            {
               existingProduct.ImageUrl= product.ImageUrl;
            }
         }
      }
   }
}
