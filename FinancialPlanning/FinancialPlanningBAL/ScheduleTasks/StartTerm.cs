


using FinancialPlanningBAL.BackgroundServices;
using FinancialPlanningBAL.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPlanningBAL.ScheduleTasks
{
    public class StartTerm : ScheduledProcessor
    {
        protected override string Schedule => "0 7 * * *"; // 7 AM every day

        public StartTerm(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            var termService = serviceProvider.GetRequiredService<ITermService>();
            var termList = await termService.getStartingTerms();
        }
    }
}