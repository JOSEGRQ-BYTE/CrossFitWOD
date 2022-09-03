using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using IdentityModel;
using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        internal DbContext _Context;
        internal DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _Context = context;
            _dbSet = context.Set<T>();
        }

        // Create/Add a new entity => POST (C)
        public virtual async Task Create(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            await _Context.SaveChangesAsync();
        }

        // Get all entities => GET (R)
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        // Get entity by ID => GET (R)
        public virtual async Task<T> GetByID<TKey>(TKey id)
        {
            var entity = await _dbSet.FindAsync(id);

            if(entity is null)
                throw new ArgumentNullException(nameof(T));

            return entity;
        }
       

        // Updates an existing entity => UPDATE (U)
        public virtual void Update(T entityToUpdate)
        {
            // NULL reference
            if (entityToUpdate is null)
                throw new ArgumentNullException(nameof(entityToUpdate));

            // Modify
            _dbSet.Attach(entityToUpdate);
            var entry = _Context.Entry(entityToUpdate);
            entry.State = EntityState.Modified;

            _Context.SaveChanges();
        }

        // Deletes an entity from the set => DELETE (D)
        public virtual async Task Delete<TKey>(TKey id)
        {
            T entityToDelete = await GetByID(id);
            Delete(entityToDelete);

            await _Context.SaveChangesAsync();
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
    }
}

