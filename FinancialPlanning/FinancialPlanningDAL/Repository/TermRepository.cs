using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialPlanningDAL.Entities;
using FinancialPlanningDAL.Data;

namespace FinancialPlanningDAL.Repositories
{
    public class TermRepository : ITermRepository
    {
        private readonly DataContext _context;

        public TermRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateTerm(Term term)
        {
            term.Id = Guid.NewGuid();
            _context.Terms!.Add(term);
            await _context.SaveChangesAsync();
            return term.Id;
        }

        public async Task DeleteTerm(Term term)
        {
            _context.Terms!.Remove(term);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Term>> GetAllTerms()
        {
            return await _context.Terms!.ToListAsync();
        }

        public async Task<Term> GetTermById(Guid id)
        {
            var term = await _context.Terms!.FindAsync(id);
            if (term == null)
            {
                throw new Exception("Term not found");
            }

            return term;
        }

        public Task UpdateTerm(Term term)
        {
            _context.Terms!.Update(term);
            return _context.SaveChangesAsync(); 
        }





        // Implement your repository methods here

    }
}
