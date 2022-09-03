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
}

