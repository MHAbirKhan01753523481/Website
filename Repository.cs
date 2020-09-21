using Alphasoft.Data;
using Alphasoft.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alphasoft.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;

        protected readonly DbSet<T> _db;

        public Repository(DbContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public T Get(int id)
        {
            return _db.Find(id);
        }

        public List<T> GetAll()
        {
            return _db.ToList();
        }

        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _db.Where(predicate).ToList();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _db.SingleOrDefault(predicate);
        }

        public T FirstOrDefault()
        {
            return _db.FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _db.FirstOrDefault(predicate);
        }

        public T LastOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _db.LastOrDefault(predicate);
        }

        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public void AddRange(List<T> entities)
        {
            _db.AddRange(entities);
        }

        public void Update(T entity)
        {
            _db.Update(entity);
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            _db.RemoveRange(entities);
        }
    }
}
