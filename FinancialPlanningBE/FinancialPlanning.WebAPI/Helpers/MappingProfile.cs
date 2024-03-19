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
           .ForMember(dest => dest.TermName, opt => opt.MapFrom(src => src.Term.TermName))         
           .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
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





            //map User to userModel
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                 .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.DOB))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                 .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.PositionName))
                 .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            //map User to AddUserModel
            CreateMap<User, AddUserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                 .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => src.DOB))
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                 .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
                 .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)).ReverseMap();

        }
    }
}