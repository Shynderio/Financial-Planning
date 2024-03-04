using System.Collections.Generic;
using FinancialPlanningDAL.Entities;

namespace FinancialPlanningDAL.Repositories
{
    public interface ITermRepository
    {
        public Task<List<Term>> GetAllTerms();
        public Task<Term> GetTermById(Guid id);
        public Task<Guid> CreateTerm(Term term);
        public Task UpdateTerm(Term term);
        public Task DeleteTerm(Term term);
    }
}
