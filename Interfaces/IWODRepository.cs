using System;
using CrossFitWOD.Models;
using Org.BouncyCastle.Asn1.X509;

namespace CrossFitWOD.Interfaces
{
    /*public interface IWODRepository: IDisposable
    {
        IEnumerable<WorkoutOfTheDay> GetStudents();
        WorkoutOfTheDay GetStudentByID(Guid id);
        void InsertStudent(WorkoutOfTheDay wod);
        void DeleteStudent(Guid id);
        void UpdateStudent(WorkoutOfTheDay wod);
        void Save();
    }*/

    public interface IWODRepository : IGenericRepository<WorkoutOfTheDay>
    {
        Task<IEnumerable<WorkoutOfTheDay>> GetWODByUserID(string id);
    }
}

