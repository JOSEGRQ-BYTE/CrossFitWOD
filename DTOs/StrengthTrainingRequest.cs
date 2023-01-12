using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
	public class StrengthTrainingRequest
	{
        [Required]
        public string? ExerciseId { get; set; }

        [Required]
        public bool IsBodyweight { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public int Reps { get; set; }

        [Required]
        public int Sets { get; set; }
    }
}

