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
         _db.Products.Update(product);
      }
   }
}
