using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PoolHockeyDAL
{
    /// <summary>    /// Generic Repository class for Entity Operations    /// </summary>    /// <typeparam name="TEntity"></typeparam>    public class GenericRepository<TEntity> where TEntity : class    {
        #region Private member variables...        internal PoolHockeyBOL.Entities Context;        internal DbSet<TEntity> DbSet;

        #endregion
        #region Public Constructor...        /// <summary>        /// Public Constructor,initializes privately declared local variables.        /// </summary>        /// <param name="context"></param>        public GenericRepository(PoolHockeyBOL.Entities context)        {            this.Context = context;            this.DbSet = context.Set<TEntity>();        }

        #endregion
        #region Public member methods...
        /// <summary>
        /// generic Get method for Entities
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity>  Get()
        {
            IQueryable<TEntity> query = DbSet;
            return query;
        }

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<TEntity> GetByID(object id)        {            return DbSet.FindAsync(id);
        }

        /// <summary>        /// generic Insert method for the entities        /// </summary>        /// <param name="entity"></param>        public virtual void Insert(TEntity entity)        {            DbSet.Add(entity);        }

        /// <summary>        /// Generic Delete method for the entities        /// </summary>        /// <param name="id"></param>        public virtual void Delete(object id)        {            TEntity entityToDelete = DbSet.Find(id);            Delete(entityToDelete);        }

        /// <summary>        /// Generic Delete method for the entities        /// </summary>        /// <param name="entityToDelete"></param>        public virtual void Delete(TEntity entityToDelete)        {            if (Context.Entry(entityToDelete).State == EntityState.Detached)            {                DbSet.Attach(entityToDelete);            }            DbSet.Remove(entityToDelete);        }

        /// <summary>        /// Generic update method for the entities        /// </summary>        /// <param name="entityToUpdate"></param>        public virtual void Update(TEntity entityToUpdate)        {            DbSet.Attach(entityToUpdate);            Context.Entry(entityToUpdate).State = EntityState.Modified;        }

        /// <summary>        /// generic method to get many record on the basis of a condition.        /// </summary>        /// <param name="where"></param>        /// <returns></returns>        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)        {            return DbSet.Where(where).ToList();        }

        /// <summary>        /// generic method to get many record on the basis of a condition but query able.        /// </summary>        /// <param name="where"></param>        /// <returns></returns>        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)        {            return DbSet.Where(where).AsQueryable();        }

        /// <summary>        /// generic get method , fetches data for the entities on the basis of condition.        /// </summary>        /// <param name="where"></param>        /// <returns></returns>        public TEntity Get(Func<TEntity, Boolean> where)        {            throw new NotSupportedException();            return DbSet.FirstOrDefault<TEntity>(where); // Should not be used : use GetFirst(Expression<Func>) with Task         }

        /// <summary>        /// generic delete method , deletes data for the entities on the basis of condition.        /// </summary>        /// <param name="where"></param>        /// <returns></returns>        public void Delete(Func<TEntity, Boolean> where)        {            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();            foreach (TEntity obj in objects)                DbSet.Remove(obj);        }

        /// <summary>        /// generic method to fetch all the records from db        /// </summary>        /// <returns></returns>        public virtual Task<List<TEntity>> GetAll()        {            return DbSet.ToListAsync();        }

        /// <summary>        /// Inclue multiple        /// </summary>        /// <param name="predicate"></param>        /// <param name="include"></param>        /// <returns></returns>        public IQueryable<TEntity> GetWithInclude(
                System.Linq.Expressions.Expression<Func<TEntity,
                bool>> predicate, params string[] include)        {            IQueryable<TEntity> query = this.DbSet;            query = include.Aggregate(query, (current, inc) => current.Include(inc));            return query.Where(predicate);        }

        /// <summary>        /// Generic method to check if entity exists        /// </summary>        /// <param name="primaryKey"></param>        /// <returns></returns>        public bool Exists(object primaryKey)        {            return DbSet.FindAsync(primaryKey) != null;        }

        /// <summary>        /// Gets a single record by the specified criteria (usually the unique identifier)        /// </summary>        /// <param name="predicate">Criteria to match on</param>        /// <returns>A single record that matches the specified criteria</returns>        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate)        {            return DbSet.SingleOrDefaultAsync<TEntity>(predicate);        }

        /// <summary>        /// The first record matching the specified criteria        /// </summary>        /// <param name="predicate">Criteria to match on</param>        /// <returns>A single record containing the first record matching the specified criteria</returns>        public Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate)        {            return DbSet.FirstOrDefaultAsync<TEntity>(predicate);        }

        #endregion    }
}
