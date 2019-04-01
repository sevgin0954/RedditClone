using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedditClone.Data.Repositories.Generic.Interfaces;

namespace RedditClone.Data.Repositories.Generic
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public BaseRepository(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        protected DbContext DbContext { get; }

        public void Add(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(params TEntity[] entities)
        {
            this.DbContext.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbContext.Set<TEntity>().Where(predicate);
        }

        public TEntity GetById(string id)
        {
            return this.DbContext.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await this.DbContext.Set<TEntity>().FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.DbContext.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.DbContext.Set<TEntity>().ToListAsync();
        }

        public void Remove(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(params TEntity[] entities)
        {
            this.DbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}
