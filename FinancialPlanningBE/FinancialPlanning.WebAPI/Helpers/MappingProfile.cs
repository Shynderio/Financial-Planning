using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.WebAPI.Models;
using FinancialPlanning.WebAPI.Models.Plan;
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
            CreateMap<LoginModel, User>();
            CreateMap<ResetPasswordModel, User>();

            // map report to  ReportViewModel
            CreateMap<Report, ReportViewModel>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.GetMaxVersion()));

            // map reportViewModel to  Report
            CreateMap<ReportViewModel, Report>();
            // Map plan 
            CreateMap<Plan, PlanListModel>();

            CreateMap<PlanListModel, Plan>();
            


            


        }
    }
}