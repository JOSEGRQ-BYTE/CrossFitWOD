using System;
namespace CrossFitWOD.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByID<TKey>(TKey id);
        IQueryable<T> GetAll();
        Task Create(T entity);
        void Update(T entity);
    }
}

