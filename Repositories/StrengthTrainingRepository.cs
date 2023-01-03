using System;
using CrossFitWOD.Data;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;

namespace CrossFitWOD.Repositories
{
	public class StrengthTrainingRepository : GenericRepository<StrengthTraining>, IStrengthTrainingRepository
    {

        public StrengthTrainingRepository(AppDBContext context) : base(context)
        {
        }
    }
}

