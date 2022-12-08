using Schedent.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Schedent.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly SchedentContext _context;

        /// <summary>
        /// Repository constructor
        /// Inject the SchedentContext
        /// </summary>
        /// <param name="context"></param>
        public Repository(SchedentContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generic method for retrieving an entity by the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Generic method for retrieving an entity by the guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public TEntity Get(Guid guid)
        {
            return _context.Set<TEntity>().Find(guid);
        }

        /// <summary>
        /// Generic method for retrieving all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Generic method for retrieving all entities with the given conditions
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// Generic method for retrieving one entity with the given conditions
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Generic method for adding a new entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Generic method for adding a list of entities
        /// </summary>
        /// <param name="entities"></param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        /// <summary>
        /// Generic method for removing an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Generic method for removing a list of entities
        /// </summary>
        /// <param name="entities"></param>
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
    }
}