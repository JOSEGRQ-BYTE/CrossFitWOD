using System;
using CrossFitWOD.Models;
using CrossFitWOD.Repositories;

namespace CrossFitWOD.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<WorkoutOfTheDay> WODRepository { get; }
        void Save();
    }
}

