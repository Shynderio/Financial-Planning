using Microsoft.Extensions.Hosting;

namespace FinancialPlanning.Service.BackgroundServices
{
    public abstract class BackgroundService : IHostedService
    {
        private Task? _executingTask;
        private CancellationTokenSource? _stoppingCts;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (_stoppingCts = new CancellationTokenSource())
            {
                _executingTask = ExecuteAsync(_stoppingCts.Token);

                if (_executingTask.IsCompleted)
                {
                    return _executingTask;
                }

                return Task.CompletedTask;
            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts?.Cancel();
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