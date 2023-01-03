using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
	public class StrengthTrainingRequest
	{
        [Required]
        public string? ExerciseId { get; set; }

        //public string? Description { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public int Reps { get; set; }

        [Required]
        public int Sets { get; set; }
    }
}

