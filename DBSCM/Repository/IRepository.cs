
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace DBSCM.Repository
{
    public interface IRepository<T>where T:class
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
		Task<List<T>> GetFromStoreAll(string SqlQu);
		
		void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IQueryable<T>> GetAllLazyLoad(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] children);
        Task SaveChangesAsync();
   
    }
}
