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
}

