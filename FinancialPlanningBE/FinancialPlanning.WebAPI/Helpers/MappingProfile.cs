using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.WebAPI.Models.Department;
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

            CreateMap<LoginModel, User>()
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<ResetPasswordModel, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));

            // map report to  ReportViewModel
            CreateMap<Report, ReportViewModel>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.GetMaxVersion()));

            // map reportViewModel to  Report
            CreateMap<ReportViewModel, Report>();

            //Map department
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();

            CreateMap<ReportViewModel, Report>()
      .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
      .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            // Map plan 
            CreateMap<Plan, PlanListModel>();

            CreateMap<PlanListModel, Plan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.PlanName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TermId, opt => opt.MapFrom(src => src.TermId))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
            CreateMap<Plan, PlanViewModel>()
            .ForMember(dest => dest.No, opt => opt.Ignore()) // Không có sẵn trong Plan, có thể bỏ qua
            .ForMember(dest => dest.Plan, opt => opt.MapFrom(src => src.PlanName))
            .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.Term.TermName))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.DepartmentName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<PlanViewModel, Plan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id sẽ được sinh ra tự động bởi cơ sở dữ liệu
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => int.Parse(src.Status)))
                .ForMember(dest => dest.TermId, opt => opt.Ignore()) // TermId và DepartmentId sẽ được đặt từ mã định danh trong PlanViewModel
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForPath(dest => dest.Term.TermName, opt => opt.MapFrom(src => src.Term))
                .ForPath(dest => dest.Department.DepartmentName, opt => opt.MapFrom(src => src.Department));





        }
    }
}