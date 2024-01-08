using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;

namespace MVC.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly DatabaseContext _db;
        internal DbSet<T> dbSet;
        public Repository(DatabaseContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        void IRepository<T>.Add(T entity)
        {
            dbSet.Add(entity);
        }
        /// <summary>
        /// Use Get query to get a single Item
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeprops"></param>
        /// <returns></returns>
        T IRepository<T>.Get(Expression<Func<T, bool>> filter, string? includeprops = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeprops))
            {
                foreach (var props in includeprops.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(props);
                }
            }
            
            return query.FirstOrDefault();
        }

        IEnumerable<T> IRepository<T>.GetAll(string? includeprops = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeprops))
            {
                foreach (var props in includeprops.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(props);
                }
            }
            return query.ToList();
        }

        void IRepository<T>.Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        void IRepository<T>.RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

    }
}
