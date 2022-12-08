using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Schedent.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Genenrtic method for retrieving an entity by the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(int id);
        /// <summary>
        /// Genenrtic method for retrieving an entity by the guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        TEntity Get(Guid guid);

        /// <summary>
        /// Genenrtic method for retrieving all entities
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Generic method for retriving all entities by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Generic method for retriving an entity by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Generic method for adding an entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Generic method for adding a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Generic method for removing an entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Generic method for removing a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
