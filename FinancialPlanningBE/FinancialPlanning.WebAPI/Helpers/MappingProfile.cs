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

            CreateMap<Report, ReportViewModel>()
           .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
           .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.TermName, opt => opt.MapFrom(src => src.Term.TermName))
           .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
           .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.GetMaxVersion()));

            CreateMap<ReportViewModel, Report>()
      .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
      .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    

        }
    }
}
