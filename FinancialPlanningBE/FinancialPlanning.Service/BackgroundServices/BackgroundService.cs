using Microsoft.Extensions.Hosting;

namespace FinancialPlanning.Service.BackgroundServices
{
    public abstract class BackgroundService : IHostedService
    {
        private Task? _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                await _stoppingCts.CancelAsync();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Process();
                await Task.Delay(5000, stoppingToken);
            } while (!stoppingToken.IsCancellationRequested);
        }

        protected abstract Task Process();
    }
}