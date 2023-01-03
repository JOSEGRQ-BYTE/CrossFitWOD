using System;
using AutoMapper;
using CrossFitWOD.DTOs;
using CrossFitWOD.Models;

namespace CrossFitWOD.Profiles
{
	public class WODProfile : Profile
	{
		public WODProfile()
		{
            CreateMap<WorkoutOfTheDay, WODResponse>();
            CreateMap<WODResponse, WorkoutOfTheDay>();

            CreateMap<WorkoutOfTheDay, WODRequest>();
            CreateMap<WODRequest, WorkoutOfTheDay>();
        }
	}
}

