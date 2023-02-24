using System;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApiApplication.Services
{
    public class ImdbBackgroundService : BackgroundService
    {
        private readonly ILogger<ImdbBackgroundService> _logger;
        private int _executionCount = 0;
        private Timer? _timer = null;
        private IServiceScopeFactory _factory;
        private IMDBStatus _imdbStatus;

        public ImdbBackgroundService(
            ILogger<ImdbBackgroundService> logger,
            IServiceScopeFactory factory,
            IMDBStatus imdbStatus)
        {
            _logger = logger;
            _factory = factory;
            _imdbStatus = imdbStatus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(GetImdbStatus, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

        }

        private async void GetImdbStatus(object state)
        {
            try
            {
                using IServiceScope scope = _factory.CreateScope();
                var imdbService = scope.ServiceProvider.GetRequiredService<IImdbService>();
                var isHealthy = await imdbService.IsHealthyAsync();

                _imdbStatus.LastCall = DateTime.UtcNow;
                _imdbStatus.Up = isHealthy;

                _executionCount++;
                _logger.LogInformation(
                    $"Executed ImdbBackgroundService - Count: {_executionCount}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute ImdbBackgroundService with exception message {ex.Message}");
            }
        }
    }
}
