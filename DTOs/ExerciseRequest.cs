using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
	public class ExerciseRequest
	{
        [Required]
        public string? ExerciseName { get; set; }

        public string? Description { get; set; }
    }
}

