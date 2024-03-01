


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
            await Task.Run(() =>
            {
                // Console.WriteLine("Task executed!" + DateTime.Now);
                var terms = termService.getStartingTerms();
                foreach (var term in terms)
                {
                    // Start the term
                    Console.WriteLine("Starting term: " + term.TermName);
                    Console.WriteLine("Start date: " + term.StartDate);
                }
            });
            
        }
    }
}