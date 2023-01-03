using System;
using AutoMapper;
using CrossFitWOD.DTOs;
using CrossFitWOD.Models;

namespace CrossFitWOD.Profiles
{
	public class StrengthTrainingProfile : Profile
	{
		public StrengthTrainingProfile()
		{
            CreateMap<StrengthTraining, StrengthTrainingResponse>();
            CreateMap<StrengthTrainingResponse, StrengthTraining>();

            CreateMap<StrengthTraining, StrengthTrainingRequest>();
            CreateMap<StrengthTrainingRequest, StrengthTraining>();
        }
	}
}

