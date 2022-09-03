using System;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace CrossFitWOD.Repositories
{

    public class WODRepository : GenericRepository<WorkoutOfTheDay>, IWODRepository
    {
        public WODRepository(AppDBContext context) : base(context)
        {

        }
        public async Task<IEnumerable<WorkoutOfTheDay>> GetWODByUserID(string id)
        {
            return await _Context.Set<WorkoutOfTheDay>().Where(w => w.UserId==id).ToListAsync();
        }
    }

    //public class WODRepository : GenericRepository<WorkoutOfTheDay>, IWODRepository
    //{
    /*public WODRepository(AppDBContext context) : base(context)
    {

    }
    public async Task<IEnumerable<WorkoutOfTheDay>> GetWODByUserID(string id)
    {
        return await _Context.WODs.Where(w => w.UserId==id).ToListAsync();
    }*/

    // From IWODRepository
    /*public Task Add(WorkoutOfTheDay model)
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
    }*/
    //}
}

