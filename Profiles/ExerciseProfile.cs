using System;
using AutoMapper;
using CrossFitWOD.DTOs;
using CrossFitWOD.Models;

namespace CrossFitWOD.Profiles
{
	public class ExerciseProfile : Profile
	{
		public ExerciseProfile()
		{
            CreateMap<Exercise, ExerciseRequest>();
            CreateMap<ExerciseRequest, Exercise>();

            CreateMap<Exercise, ExerciseResponse>();
            CreateMap<ExerciseResponse, Exercise>();
        }
	}
}

