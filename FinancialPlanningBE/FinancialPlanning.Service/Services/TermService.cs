using System;
using System.Collections.Generic;
using System.Threading.Tasks; // Add this namespace
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinancialPlanning.Service.Services
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
            List<Term> startingTerms = [];
            foreach (var term in terms)
            {
                if (term.StartDate >= DateTime.Now.AddDays(-7) && term.Status == 0)
                {
                    startingTerms.Add(term);
                }
            }
            return startingTerms;
        }

        public async Task<Term> GetTermById(Guid id)
        {
            return await _termRepository.GetTermById(id);
        }
        public async Task StartTerm(Guid id)
        {
            var term = await _termRepository.GetTermById(id);
            if (term != null)
            {
                term.Status = 1;
                await _termRepository.UpdateTerm(term);
            }
            else
            {
                throw new ArgumentException("Term not found with the specified ID");
            }
        }

        public async Task CreateTerm(Term term)
        {
            term.Status = 0;
            var endDate = term.StartDate.AddMonths(term.Duration);
            if (endDate < term.ReportDueDate){
                throw new ArgumentException("Report due date cannot be after the end date");
            } else
            if (endDate < term.PlanDueDate){
                throw new ArgumentException("Plan due date cannot be after the end date");
            } else
                await _termRepository.CreateTerm(term);
        }

        public async Task UpdateTerm(Term term)
        {
            var existingTerm = await _termRepository.GetTermById(term.Id) ?? throw new ArgumentException("Term not found with the specified ID");

            var Status = existingTerm.Status;
            if (Status == 1)
            {
                existingTerm.TermName = term.TermName;
                existingTerm.CreatorId = term.CreatorId;
                existingTerm.Duration = term.Duration;
                existingTerm.StartDate = term.StartDate;
                existingTerm.PlanDueDate = term.PlanDueDate;
                existingTerm.ReportDueDate = term.ReportDueDate;
                await _termRepository.UpdateTerm(term);
            }
            else
            {
                throw new ArgumentException("Term cannot be updated as it is not in the new status");
            }
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

        public async Task<IEnumerable<Term>> GetAllTerms()
        {
            return await _termRepository.GetAllTerms();
        }

        public async Task CloseTerms()
        {
            IEnumerable<Term> terms = await _termRepository.GetAllTerms();
            foreach (var term in terms)
            {
                var endDate = term.StartDate.AddMonths(term.Duration);
                if (endDate <= DateTime.Now && term.Status == 1)
                {
                    term.Status = 3;
                    await _termRepository.UpdateTerm(term);
                }
            }
        }
    }
}
