﻿using System;
using CrossFitWOD.Models;

namespace CrossFitWOD.DTOs
{
    public record WorkoutOfTheDayDTO
    {
        public Guid Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public WODLevel Level { get; init; }
        public DateTime Date { get; init; }
        public string? CoachTip { get; init; }
    }
}

