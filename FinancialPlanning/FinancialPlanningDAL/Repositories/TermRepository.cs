using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinancialPlanningDAL.Entities;

namespace FinancialPlanningDAL.Repositories
{
    public class TermRepository : ITermRepository
    {
        // private readonly DataContext _context;

        // public TermRepository(DbContext context)
        // {
        //     _context = context;
        // }

        public void AddTerm(Term term)
        {
            throw new NotImplementedException();
        }

        public void DeleteTerm(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> GetAllTerms()
        {
            return Enumerable.Range(1, 5).Select(i => new Term
            {
                Id = Guid.NewGuid(), // Generate a new GUID for each Term
                TermName = $"Term {i}", // Assign a unique name for each Term
                CreatorId = Guid.NewGuid(), // Generate a new GUID for the creator
                Duration = 3, // Fill in with appropriate values
                StartDate = DateTime.Now.AddDays(-2 * i), // Adjust start date as needed
                PlanDueDate = DateTime.Now.AddDays(7 - i), // Adjust plan due date as needed
                ReportDueDate = DateTime.Now.AddDays(14 - i), // Adjust report due date as needed
                Status = 1 // Set the status as needed
                           // You can fill in other properties as needed
            });
        }


        public Term GetTermById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTerm(Term term)
        {
            throw new NotImplementedException();
        }



        // Implement your repository methods here

    }
}
