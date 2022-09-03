using System;
using CrossFitWOD.DTOs;
using CrossFitWOD.Models;

namespace CrossFitWOD.Extensions
{
    public static class DataTransferObjectConvertion
    {
        public static WorkoutOfTheDayDTO ToDTO(this WorkoutOfTheDay wod)
        {
            return new WorkoutOfTheDayDTO
            {
               // WOD Info
               Id = wod.Id,
               Title = wod.Title,
               Description = wod.Description,
               CoachTip = wod.CoachTip,
               Level = wod.Level,
               Date = wod.Date,
               Results = wod.Results,

               // User Info
               //FirstName = wod.User.FirstName,
               //LastName = wod.User.LastName,
            };
        }
    }
}

