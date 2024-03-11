using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.WebAPI.Models;
namespace FinancialPlanning.WebAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Term, TermViewModel>().ReverseMap();
            CreateMap<CreateTermModel, Term>();
            CreateMap<LoginModel, User>()
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
        } 
    }
}
