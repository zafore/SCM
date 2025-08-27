using Microsoft.EntityFrameworkCore;

namespace DBSCM.Repository
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        protected readonly DBSCM.Context.SCMDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EntityFrameworkRepository(DBSCM.Context.SCMDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();
        public T? GetById(int id) => _dbSet.Find(id);
        public void Add(T entity) => _dbSet.Add(entity);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null) _dbSet.Remove(entity);
        }
        public void UpdateDatabase() => _context.SaveChanges();
    }
}
