using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.DataAcess.Data;
using Bookstore.Models;

namespace Bookstore.DataAccess.Repository
{
   public class CategoryRepository : Repository<Category>, ICategoryRepository
   {
      private ApplicationDbContext _db;
      public CategoryRepository(ApplicationDbContext db) : base(db)
      {
         _db = db;
      }

      public void Update(Category category)
      {
         _db.Categories.Update(category);
      }
   }
}
