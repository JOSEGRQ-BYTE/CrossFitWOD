﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.Models
{
	public record StrengthTraining
	{
        [Key]
        [Required]
        public string? Id { get; set; }

        [Required]
        public string? ExerciseId{ get; set; }

        [Required]
        public bool IsBodyweight { get; set; }

        public int Weight { get; set; }

        [Required]
        public int Reps { get; set; }

        [Required]
        public int Sets { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}

