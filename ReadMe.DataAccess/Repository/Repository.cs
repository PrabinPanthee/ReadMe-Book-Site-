using Microsoft.EntityFrameworkCore;
using ReadMe.DataAccess.Data;
using ReadMe.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;
        public Repository ( ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();

            //for navigation property or accessing one entity from another
            //_db.products.Include(u => u.Category);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> Filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
          
            query =  query.Where(Filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.
                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }

            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.
                    Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries) )
                {
                 query = query.Include(property);
                }
            
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
           _db.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           _db.RemoveRange(entities);
        }
    }
}
