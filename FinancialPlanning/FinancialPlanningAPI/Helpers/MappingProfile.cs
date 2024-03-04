﻿using AutoMapper;
using FinancialPlanningAPI.Models;
using FinancialPlanningDAL.Entities;

namespace FinancialPlanningAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginModel, User>()
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        }

    }

    
}
