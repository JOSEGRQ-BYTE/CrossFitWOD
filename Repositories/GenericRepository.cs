using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        internal DbContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Create/Add a new entity => POST (C)
        public virtual async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Get all entities => GET (R)
        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        // Get entity by ID => GET (R)
        public virtual async Task<T> GetByID<TKey>(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Updates an existing entity => UPDATE (U)
        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        // Deletes an entity from the set => DELETE (D)
        public virtual async Task Delete<TKey>(TKey id)
        {
            T entityToDelete = await GetByID(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
    }
}

