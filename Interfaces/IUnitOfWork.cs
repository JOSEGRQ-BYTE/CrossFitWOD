using System;
using CrossFitWOD.Data;
using CrossFitWOD.Models;
using CrossFitWOD.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IWODRepository WODRepository { get; }
        void Save();
    }


    /*public interface IUnitOfWork<out TContext> where TContext : AppDBContext, new()
    {
        TContext GetContext { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        void SaveChanges();
    }*/
}

