using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace VadgerWorkspace.Data.Repositories
{
    public class BaseRepository<T> : IDisposable, IBaseRepository<T> where T : class
    {
        protected DbContext _dbContext;

        public BaseRepository(DbContext dbContext) 
        { 
            _dbContext = dbContext; 
        }

        public async void Create(T entity) 
        { 
            await _dbContext.Set<T>().AddAsync(entity); 
        }

        public void Delete(T entity) 
        { 
            _dbContext.Set<T>().Remove(entity); 
        }

        public IQueryable<T> FindAll() 
            => _dbContext.Set<T>().AsNoTracking(); 

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) 
            => _dbContext.Set<T>().Where(expression).AsNoTracking();


        public async Task SaveAsync() 
        { 
            await _dbContext.SaveChangesAsync(); 
        }

        public void Update(T entity) 
        { 
            _dbContext.Set<T>().Update(entity);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        #endregion
    }
}
