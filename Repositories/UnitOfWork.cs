using System;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        private AppDBContext _Context;
        private GenericRepository<WorkoutOfTheDay> _WODRepository;
        private bool _Disposed = false;

        public UnitOfWork(AppDBContext context)
        {
            _Context = context;
            _WODRepository = new GenericRepository<WorkoutOfTheDay>(_Context);
        }

        public GenericRepository<WorkoutOfTheDay> WODRepository
        {
            get
            {

                if(_WODRepository is null)
                    _WODRepository = new GenericRepository<WorkoutOfTheDay>(_Context);
                return _WODRepository;
            }
        }

        public void Save()
        {
            _Context.SaveChanges();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            _Disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

