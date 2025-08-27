

using DBSCM.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//https://dotnettutorials.net/lesson/unit-of-work-csharp-mvc/

namespace DBSCM.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
       
        protected readonly SCMDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(SCMDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task<List<T>> GetAll()
        {
            return _dbSet.ToListAsync();
        }
		public async Task<List<T>> GetFromStoreAll(string SqlQu)
		{
			return await _context.Database
			.SqlQueryRaw<T>(SqlQu)
			.ToListAsync();
			// _dbSet.FromSqlRaw(SqlQu).ToListAsync();
		}
		//public async Task<List<T>> GetAllLazyLoad(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] children)
		//{


		//    children.ToList().ForEach(x => _dbSet.Include(x).Load());
		//    return await _dbSet.ToListAsync();
		//}
		public async Task<IQueryable<T>> GetAllLazyLoad ( Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
    {

		IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply Include expressions if provided
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

           return await Task.FromResult(query);
    }

        public void Add(T entity)
        {

            _dbSet.Add(entity);
           
        }

        public void Update(T entity)
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
