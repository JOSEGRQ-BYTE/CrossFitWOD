using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.Models
{
	public class Exercise
	{
		[Key]
		[Required]
		public string? Id { get; set; }

        [Required]
        public string? ExerciseName { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}

