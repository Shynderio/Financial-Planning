using System;
using System.Collections.Generic;
using FinancialPlanningDAL.Entities;
using FinancialPlanningDAL.Repositories;

namespace FinancialPlanningBAL.Services
{
    public class TermService
    {
        private readonly ITermRepository _termRepository;

        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public IEnumerable<Term> getStartingTerms(){
            IEnumerable<Term> terms = _termRepository.GetAllTerms();
            foreach (var term in terms)
            {
                if (term.StartDate >= DateTime.Now.AddDays(-7) && term.Status == 1)
                {
                    yield return term;
                }
            }
        }
    }
}
