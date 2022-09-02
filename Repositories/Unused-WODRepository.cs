using System;
using CrossFitWOD.Data;
using CrossFitWOD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Repositories
{
    public class WODRepository : IDisposable
    {
        // From IWODRepository
        public Task Add(WorkoutOfTheDay model)
        {
            throw new NotImplementedException();
        }

        public void Delete(WorkoutOfTheDay model)
        {
            throw new NotImplementedException();
        }

        public Task<WorkoutOfTheDay> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkoutOfTheDay>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Guid id, WorkoutOfTheDay model)
        {
            throw new NotImplementedException();
        }

        // From IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

