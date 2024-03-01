using System.Collections.Generic;
using FinancialPlanningDAL.Entities;

namespace FinancialPlanningDAL.Repositories
{
    public interface ITermRepository
    {
        Term GetTermById(int id);
        IEnumerable<Term> GetAllTerms();
        void AddTerm(Term term);
        void UpdateTerm(Term term);
        void DeleteTerm(int id);
    }
}
