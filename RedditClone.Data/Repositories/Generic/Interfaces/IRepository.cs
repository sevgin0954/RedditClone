using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RedditClone.Data.Repositories.Generic.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(string id);
        Task<TEntity> GetByIdAsync(string id);
        IEnumerable<TEntity> GetAll();
        IQueryable<TEntity> GetAllAsQueryable();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(params TEntity[] entities);

        void Remove(TEntity entity);
        void RemoveRange(params TEntity[] entities);
    }
}
