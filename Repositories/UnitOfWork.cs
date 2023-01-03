using System;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CrossFitWOD.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {

        private AppDBContext _Context;
        private bool _Disposed = false;
        public IWODRepository WODRepository { get; }
        public IStrengthTrainingRepository StrengthTrainingRepository { get; }
        public IExerciseRepository ExerciseRepository { get; }


        public UnitOfWork(AppDBContext dBContext, IWODRepository wODRepository, IStrengthTrainingRepository strengthTrainingRepository, IExerciseRepository exerciseRepository)
        {
            _Context = dBContext;
            WODRepository = wODRepository;
            StrengthTrainingRepository = strengthTrainingRepository;
            ExerciseRepository = exerciseRepository;
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

