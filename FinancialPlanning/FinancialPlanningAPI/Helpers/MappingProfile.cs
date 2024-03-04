using AutoMapper;
using FinancialPlanningDAL.Entities;
using FinancialPlanningAPI.Models;
using FinancialPlanningAPI.ViewModels;
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