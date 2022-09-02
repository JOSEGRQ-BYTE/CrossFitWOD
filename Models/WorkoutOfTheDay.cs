using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.Models
{
    // init will set the porperty ONCE (during initialization) and never again
    public record WorkoutOfTheDay
    {
        public Guid Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public WODLevel Level { get; init; }
        public DateTime Date { get; init; }
        public string? CoachTip { get; init; }
        public string? Results { get; init; }
    }
}

