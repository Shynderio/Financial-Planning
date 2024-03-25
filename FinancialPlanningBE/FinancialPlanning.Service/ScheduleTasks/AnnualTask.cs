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

        protected override string Schedule => "*/3 * * * *"; 
        // protected override string Schedule => "0 0 20 12 *";

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            // var termService = serviceProvider.GetRequiredService<TermService>();
            var reportService = serviceProvider.GetRequiredService<ReportService>();
            await reportService.GenerateAnnualReport();
            
        }
    }
}