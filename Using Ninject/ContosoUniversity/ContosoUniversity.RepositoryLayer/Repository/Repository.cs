using ContosoUniversity.DataAccessLayer;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContosoUniversity.RepositoryLayer.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SchoolContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            /*string includeProperties = "",*/ params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            //foreach (
            //    var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    query = query.Include(includeProperty);
            //}

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;

        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync();
        }

        public void InsertEntity(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void UpdateEntity(TEntity entity, params string[] excludeProperties)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            foreach (string excludeProperty in excludeProperties)
            {
                _dbContext.Entry(entity).Property(excludeProperty).IsModified = false;
            }
        }
        public void DeleteEntity(object id)
        {
            TEntity entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

    }

}