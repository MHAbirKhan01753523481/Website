using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alphasoft.IRepositories
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);

        List<T> GetAll();

        List<T> Find(Expression<Func<T, bool>> predicate);

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        T FirstOrDefault();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        
        T LastOrDefault(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void AddRange(List<T> entities);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(List<T> entities);
    }
}
