using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Add this namespace
using FinancialPlanningDAL.Entities;
using FinancialPlanningDAL.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinancialPlanningBAL.Services
{
    public class TermService
    {
        private readonly ITermRepository _termRepository;

        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public async Task<IEnumerable<Term>> GetStartingTerms()
        {
            IEnumerable<Term> terms = await _termRepository.GetAllTerms();
            foreach (var term in terms)
            {
                if (term.StartDate >= DateTime.Now.AddDays(-7) && term.Status == 1)
                {
                    terms = terms.Append(term);
                }
            }
            return terms;
        }

        public async Task StartTerm(Term term)
        {
            if (term.Status != 1)
            {
                throw new Exception("Term is not in the correct status to be started");
            }
            else
            {
                term.Status = 2;
                await _termRepository.UpdateTerm(term);
            }
        }

        public async Task CreateTerm(Term term)
        {
            term.Status = 1;
            await _termRepository.CreateTerm(term);
        }

        public async Task UpdateTerm(Term term)
        {
            await _termRepository.UpdateTerm(term);
        }

        public async Task DeleteTerm(Guid id)
        {
            var termToDelete = await _termRepository.GetTermById(id);
            if (termToDelete != null)
            {
                await _termRepository.DeleteTerm(termToDelete);
            }
            else
            {
                throw new ArgumentException("Term not found with the specified ID");
            }
        }
    }
}
