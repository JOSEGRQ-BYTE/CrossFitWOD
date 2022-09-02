using System;
using System.ComponentModel.DataAnnotations;
using CrossFitWOD.Models;

namespace CrossFitWOD.DTOs
{
    public record CreateWorkoutOfTheDayDTO
    {
        [Required]
        public string? Title { get; init; }
        [Required]
        public string? Description { get; init; }
        [Required]
        public WODLevel Level { get; init; }
        [Required]
        public DateTime Date { get; init; }
        public string? CoachTip { get; init; }
        [Required]
        public string? Results { get; init; }
    }
}

