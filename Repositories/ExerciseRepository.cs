using System;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;

namespace CrossFitWOD.Repositories
{
	public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(AppDBContext context) : base(context)
        {
        }
    }
}

