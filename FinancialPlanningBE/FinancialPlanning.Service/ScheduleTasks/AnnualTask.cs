using FinancialPlanning.Service.BackgroundServices;
using FinancialPlanning.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialPlanning.Service.ScheduleTasks
{
    public class AnnualTask : ScheduledProcessor
    {
        private readonly ILogger<StartTerm> _logger;

        public AnnualTask(IServiceScopeFactory serviceScopeFactory, ILogger<StartTerm> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }

        //protected override string Schedule => "*/1 * * * *"; // every 3 minute for testing
        protected override string Schedule => "0 0 20 12 *";

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {

            var termService = serviceProvider.GetRequiredService<AnnualReportService>();
           await termService.GenerateAnnualReport();

            
        }
    }
}