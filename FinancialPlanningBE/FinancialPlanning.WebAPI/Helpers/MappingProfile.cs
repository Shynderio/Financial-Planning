using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.WebAPI.Models.Report;
using FinancialPlanning.WebAPI.Models.Term;
using FinancialPlanning.WebAPI.Models.User;

namespace FinancialPlanning.WebAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Term, TermListModel>()
                .ForMember(dest => dest.EndDate,
                    opt => opt.MapFrom(src => src.StartDate.AddMonths(src.Duration)))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src =>
                        EntityMaps.StatusMap.ContainsKey(src.Status)
                            ? EntityMaps.StatusMap[src.Status]
                            : string.Empty));
            CreateMap<Term, TermViewModel>().ReverseMap();
            CreateMap<CreateTermModel, Term>();
            CreateMap<LoginModel, User>()
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<ResetPasswordModel, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));

            // map report to  ReportViewModel
            CreateMap<Report, ReportViewModel>()
           .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
           .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
           .ForMember(dest => dest.TermName, opt => opt.MapFrom(src => src.Term.TermName))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
           .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
           .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.GetMaxVersion()));

            // map reportViewModel to  Report
            CreateMap<ReportViewModel, Report>()
      .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
      .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));



        }
    }
}