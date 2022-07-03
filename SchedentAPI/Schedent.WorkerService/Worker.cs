using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Schedent.BusinessLogic.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Schedent.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ImportService _importService;

        public Worker(ILogger<Worker> logger, ImportService importService)
        {
            _logger = logger;
            _importService = importService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                    await _importService.ImportTimeTableAsync();
                }
            } catch (Exception ex)
            {
                _logger.LogError("Error: ", ex);
            }
        }
    }
}
