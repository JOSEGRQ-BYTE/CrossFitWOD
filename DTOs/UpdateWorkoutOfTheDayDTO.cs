using System;
using CrossFitWOD.Models;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
    public record UpdateWorkoutOfTheDayDTO
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
    }
}

