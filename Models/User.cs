using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace CrossFitWOD.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePictureURL { get; set; }

        public ICollection<WorkoutOfTheDay>? Workouts { get; set; }
    }
}

