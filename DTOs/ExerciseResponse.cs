using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
	public class ExerciseResponse
	{
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? ExerciseName { get; set; }

        public string? Description { get; set; }
    }
}

