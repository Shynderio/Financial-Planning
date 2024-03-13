using AutoMapper;
using FinancialPlanning.Data.Entities;
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
        }
    }
}