using AutoMapper;
using FinancialPlanningDAL.Entities;
using FinancialPlanningAPI.Models;
namespace FinancialPlanningAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Term, TermViewModel>().ReverseMap();
            CreateMap<CreateTermModel, Term>();
        } 
    }
}