using FinancialPlanning.Common;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;

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
                if (term.StartDate >= DateTime.Now.AddDays(-7) && term.Status == (int)TermStatus.New)
                {
                    startingTerms.Add(term);
                }
            }
            return startingTerms;
        }

        public async Task<Term?> GetTermByIdAsync(Guid id)
        {
            return await _termRepository.GetTermByIdAsync(id);
        }

        public Term GetTermById(Guid id)
        {
            return _termRepository.GetTermById(id);
        }
        public async Task StartTerm(Guid id)
        {
            var term = await _termRepository.GetTermByIdAsync(id);
            if (term != null)
            {
                term.Status = (int)TermStatus.InProgress;
                await _termRepository.UpdateTerm(term);
            }
            else
            {
                throw new ArgumentException("Term not found with the specified ID");
            }
        }

        public async Task CreateTerm(Term term)
        {
            term.Status = (int)TermStatus.New;
            var endDate = term.StartDate.AddMonths(term.Duration);
            if (endDate < term.ReportDueDate)
            {
                throw new ArgumentException("Report due date cannot be after the end date");
            }

            if (endDate < term.PlanDueDate){
                throw new ArgumentException("Plan due date cannot be after the end date");
            }

            await _termRepository.CreateTerm(term);
        }

        public async Task UpdateTerm(Term term)
        {
            var existingTerm = await _termRepository.GetTermByIdAsync(term.Id) ?? throw new ArgumentException("Term not found with the specified ID");

            var status = existingTerm.Status;
            if (status == (int)TermStatus.New)
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
            var termToDelete = await _termRepository.GetTermByIdAsync(id);
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
                if (endDate > DateTime.Now || term.Status != (int)TermStatus.InProgress) continue;
                term.Status = (int)TermStatus.Closed;
                await _termRepository.UpdateTerm(term);
            }
        }

        public async Task<IEnumerable<Term>> GetStartedTerms()
        {
            IEnumerable<Term> terms = await _termRepository.GetAllTerms();
            List<Term> startedTerms = [];
            foreach (var term in terms)
            {
                if (term.Status == (int)TermStatus.InProgress && term.Plans.Count == 0)
                {
                    startedTerms.Add(term);
                }
            }
            return startedTerms;
        }
    }
}
