using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrossFitWOD.Models
{
    // init will set the porperty ONCE (during initialization) and never again
    public record WorkoutOfTheDay
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public WODLevel Level { get; set; }
        public DateTime Date { get; set; }
        public string? CoachTip { get; set; }
        public string? Results { get; set; }



        public string? UserId { get; set; }
        public User? User { get; set; }

    }
}

