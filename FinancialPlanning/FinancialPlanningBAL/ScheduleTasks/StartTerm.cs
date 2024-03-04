


using FinancialPlanningBAL.BackgroundServices;
using FinancialPlanningBAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPlanningBAL.ScheduleTasks
{
    public class StartTerm : ScheduledProcessor
    {
        protected override string Schedule => "*/1 * * * *"; // every minute
        
        // 23:59 every day

        public StartTerm(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            var termService = serviceProvider.GetRequiredService<TermService>();
            
                var terms = await termService.GetStartingTerms();
                if (terms == null)
                {
                    return;
                }
                var termsList = terms.ToList();
                foreach (var term in termsList)
                {
                    // Start the term
                    System.Console.WriteLine("Starting term: " + term.TermName);
                    await termService.StartTerm(term);
                }
           
        }
    }
}