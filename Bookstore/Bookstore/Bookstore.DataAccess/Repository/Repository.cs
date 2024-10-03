﻿using System.Linq.Expressions;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.DataAcess.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DataAccess.Repository
{
   public class Repository<T> : IRepository<T> where T : class
   {
      private readonly ApplicationDbContext _db;

      internal DbSet<T> dbSet;

      public Repository(ApplicationDbContext db)
      {
         _db = db;
         dbSet = _db.Set<T>();
         //equicalent to _db.Categories == dbSet;
         _db.Products.Include(p => p.Category);
      }

      public void Add(T entity)
      {
         dbSet.Add(entity);
      }

      public void Delete(T entity)
      {
         dbSet.Remove(entity);
      }

      public void DeleteRange(IEnumerable<T> entity)
      {
         dbSet.RemoveRange(entity);
      }

      public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
      {
         IQueryable<T> query = dbSet;
         query = query.Where(filter);
         if (!string.IsNullOrEmpty(includeProperties))
         {
            foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
               query = query.Include(property);
            }
         }
         return query.FirstOrDefault();
      }

      public IEnumerable<T> GetAll(string? includeProperties = null)
      {
         IQueryable<T> query = dbSet;
         if (!string.IsNullOrEmpty(includeProperties))
         {
            foreach (var property in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
            {
               query = query.Include(property);
            }
         }
         return query.ToList();
      }
   }
}
