using System;
using CrossFitWOD.Models;

namespace CrossFitWOD.Interfaces
{
    public interface IWODRepository: IDisposable
    {
        IEnumerable<WorkoutOfTheDay> GetStudents();
        WorkoutOfTheDay GetStudentByID(Guid id);
        void InsertStudent(WorkoutOfTheDay wod);
        void DeleteStudent(Guid id);
        void UpdateStudent(WorkoutOfTheDay wod);
        void Save();
    }
}

