using FinancialPlanning.Service.BackgroundServices;
using FinancialPlanning.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinancialPlanning.Service.ScheduleTasks
{
    public class StartTerm : ScheduledProcessor
    {
        private readonly ILogger<StartTerm> _logger;

        public StartTerm(IServiceScopeFactory serviceScopeFactory, ILogger<StartTerm> logger) : base(serviceScopeFactory)
        {
            _logger = logger;
        }

        protected override string Schedule => "*/3 * * * *"; // every minute

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            var termService = serviceProvider.GetRequiredService<TermService>();

            var terms = await termService.GetStartingTerms();

            foreach (var term in terms)
            {
                try
                {
                    await termService.StartTerm(term.Id);
                    _logger.LogInformation($"Started term with ID {term.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error starting term with ID {term.Id}");
                }
            }
        }
    }
}