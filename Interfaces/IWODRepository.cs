using System;
using CrossFitWOD.Models;
using Org.BouncyCastle.Asn1.X509;

namespace CrossFitWOD.Interfaces
{
    public interface IWODRepository : IGenericRepository<WorkoutOfTheDay>
    {
        Task<IEnumerable<WorkoutOfTheDay>> GetWODByUserID(string id);
    }
}

