using System;
namespace CrossFitWOD.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByID<TKey>(TKey id);
        IQueryable<T> GetAll();
        Task Create(T entity);
        void Update(T entity);
        Task Delete<TKey>(TKey id);
    }
}

